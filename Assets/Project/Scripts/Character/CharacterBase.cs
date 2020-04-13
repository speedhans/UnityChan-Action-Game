using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sunTT;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterBase : MonoBehaviour
{
    static public readonly int m_AnimKeyIdle = Animator.StringToHash("Idle");
    static public readonly int m_AnimKeyMoveDirectionX = Animator.StringToHash("MoveDirectionX");
    static public readonly int m_AnimKeyMoveDirectionY = Animator.StringToHash("MoveDirectionY");
    static public readonly int m_AnimKeyDead = Animator.StringToHash("Dead");

    public enum E_Team
    {
        RED,
        BLUE,
        GREEN,
    }

    public E_Team m_Team = E_Team.RED;

    bool DestoryThisGameObject = false;

    public enum E_State
    {

    }

    public enum E_Live
    {
        LIVE,
        DEAD,
    }


    public E_Live m_Live = E_Live.LIVE;

    public Animator m_Animator { get; protected set; }
    public AnimationCallbackEvent m_AnimCallback { get; protected set; }
    public Rigidbody m_Rigidbody { get; protected set; }
    public CapsuleCollider m_CapsuleCollider { get; protected set; }
    public SkinnedMeshRenderer[] m_Renderers { get; protected set; }

    [SerializeField]
    GameObject m_Model;

    public Transform m_LeftHandPoint { get; protected set; }
    public Transform m_RightHandPoint { get; protected set; }

    protected List<CharacterBaseComponent> m_ComponentList = new List<CharacterBaseComponent>();

    protected bool m_IsAI = false;
    public bool IsAI { get { return m_IsAI; } }
    public int m_CharacterID;
    public float m_Health;
    public float m_HealthMax;

    [HideInInspector]
    public bool m_ActiveMotionRunning = false;
    protected float m_MotionCancelDelay = 0.0f;
    public bool IsActiveMotionRunnung { get { return m_ActiveMotionRunning; } }
    [HideInInspector]
    public bool m_IsDashing = false;

    public LayerMask m_EnemyLayerMask;
    public AudioClip[] m_AudioList;
    protected virtual void Awake()
    {
        if (!m_Model) return;

        GameObject model = Instantiate(m_Model, transform);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.identity;

        m_Animator = GetComponentInChildren<Animator>();
        m_AnimCallback = m_Animator.gameObject.AddComponent<AnimationCallbackEvent>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_CapsuleCollider = GetComponent<CapsuleCollider>();
        m_Renderers = m_Animator.GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    public T SetComponent<T>() where T : CharacterBaseComponent, new()
    {
        var component = new T();
        m_ComponentList.Add(component);
        return component;
    }

    public T SetComponent<T>(CharacterBase _CharacterBase) where T : CharacterBaseComponent, new()
    {
        var component = new T();
        component.Initialize(_CharacterBase);
        m_ComponentList.Add(component);
        return component;
    }

    public T FindComponent<T>() where T : CharacterBaseComponent
    {
        for (int i = 0; i < m_ComponentList.Count; ++i)
        {
            if (m_ComponentList[i].GetType() == typeof(T))
            {
                return (T)m_ComponentList[i];
            }
        }

        return null;
    }

    public void RemoveComponent<T>() where T : CharacterBaseComponent
    {
        for (int i = 0; i < m_ComponentList.Count; ++i)
        {
            if (m_ComponentList[i].GetType() == typeof(T))
            {
                m_ComponentList[i].DestoryComponent();
                m_ComponentList.RemoveAt(i);
                break;
            }
        }
    }
    protected Transform FindBone(Transform _Transform, string _BoneName)
    {
        if (_Transform.name.Contains(_BoneName)) return _Transform;

        for (int i = 0; i < _Transform.childCount; ++i)
        {
            Transform t = FindBone(_Transform.GetChild(i), _BoneName);
            if (t)
            {
                return t;
            }
        }

        return null;
    }

    protected virtual void Update()
    {
        float deltatime = Time.deltaTime;
        for (int i = 0; i < m_ComponentList.Count; ++i)
        {
            m_ComponentList[i].UpdateComponent(deltatime);
        }

        if (m_MotionCancelDelay > 0.0f)
        {
            m_MotionCancelDelay -= deltatime;
        }
    }

    protected virtual void FixedUpdate()
    {
        float deltatime = Time.fixedDeltaTime;
        for (int i = 0; i < m_ComponentList.Count; ++i)
        {
            m_ComponentList[i].FixedUpdateComponent(deltatime);
        }
    }

    protected virtual void LateUpdate()
    {
        float deltatime = Time.deltaTime;
        for (int i = 0; i < m_ComponentList.Count; ++i)
        {
            m_ComponentList[i].LateUpdateComponent(deltatime);
        }
    }

    public void SetMaterialsAlpha(float _Alpha)
    {
        foreach(SkinnedMeshRenderer s in m_Renderers)
        {
            foreach (Material m in s.materials)
            {
                Color color = m.color;
                color.a = _Alpha;
                m.color = color;
            }
        }
    }

    public void GiveToDamage(int _AttackerID, float _Damage)
    {
        if (m_Live == E_Live.DEAD) return;

        m_Health -= _Damage;
        if (m_Health <= 0.0f)
        {
            m_Live = E_Live.DEAD;
            m_Animator.CrossFade(m_AnimKeyDead, 0.15f);
        }
    }

    public void StartNewMotion()
    {
        m_ActiveMotionRunning = true;
        m_MotionCancelDelay = 0.15f;
    }

    public bool CheckMotionCancelAvailability(int _DecreaseGauge = 2)
    {
        if (GameManager.Instacne.m_Main.IsPlayStop() || GameManager.Instacne.m_Main.IsGameStop()) return false;

        if (m_ActiveMotionRunning &&
            m_MotionCancelDelay <= 0.0f &&
            UIManager.Instacne.m_MotionCancelGauge.GetGauge() > 1)
        {
            if (_DecreaseGauge > 0)
                UIManager.Instacne.m_MotionCancelGauge.SubGauge(_DecreaseGauge);
            return true;
        }

        return false;
    }

    public virtual void MotionEnd(int _Value)
    {
        m_ActiveMotionRunning = false;
    }

    public void Revive()
    {
        m_Health = m_HealthMax;
        m_Live = E_Live.LIVE;
        m_Animator.CrossFade(m_AnimKeyIdle, 0.15f);
    }
    private void OnDestroy()
    {
        if (DestoryThisGameObject) return;

        foreach(CharacterBaseComponent c in m_ComponentList)
        {
            c.DestoryComponent();
        }
        m_ComponentList.Clear();
    }

    private void OnApplicationQuit()
    {
        DestoryThisGameObject = true;
    }
}
