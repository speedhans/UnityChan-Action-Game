using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sunTT;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterBase : MonoBehaviour, IDamage
{
    static public readonly int m_AnimKeyIdle = Animator.StringToHash("Idle/MoveTree");
    static public readonly int m_AnimKeyMoveDirectionX = Animator.StringToHash("MoveDirectionX");
    static public readonly int m_AnimKeyMoveDirectionY = Animator.StringToHash("MoveDirectionY");
    static public readonly int m_AnimKeyDeath = Animator.StringToHash("Death");
    static public readonly int m_AnimKeyHit = Animator.StringToHash("Hit");
    static public readonly int m_AnimKeyHitAirStart = Animator.StringToHash("HitAir start");
    static public readonly int m_AnimKeyHitAirEnd = Animator.StringToHash("HitAir end");

    public enum E_Team
    {
        RED,
        BLUE,
        GREEN,
    }

    public E_Team m_Team = E_Team.RED;

    bool DestoryThisGameObject = false;

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
    [HideInInspector]
    public bool m_HitMotion = false;
    [HideInInspector]
    public bool m_Down = false;
    [SerializeField]
    protected bool m_UseHitAnimation = false;

    [HideInInspector]
    public bool m_Immortal = false;
    [HideInInspector]
    public bool m_StopCharacter = false;

    public LayerMask m_EnemyLayerMask;
    public AudioClip[] m_AudioList;
    public AudioClip[] m_AudioListHit;
    public AudioClip[] m_AudioListFoot;
    public AudioClip m_DeathClip;

    protected virtual void Awake()
    {
        m_Animator = GetComponentInChildren<Animator>();
        if (!m_Animator)
        {
            if (!m_Model) return;
            GameObject model = Instantiate(m_Model, transform);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;
            m_Animator = GetComponentInChildren<Animator>();
        }

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

    public virtual void GiveToDamage(CharacterBase _Attacker, float _Damage, bool _Knockback = false)
    {
        if (m_Live == E_Live.DEAD) return;
        if (m_Immortal) return;
        if (m_Down) return;
        if (_Attacker.m_CharacterID == 0)
        {
            UIManager.Instacne.m_ComboViewer.AddCombo();
            Vector3 pos = _Attacker.transform.position + _Attacker.transform.forward + (Vector3.up * 1.5f) + (_Attacker.transform.right * 0.5f);
            Billboard damage = ObjectPool.GetObject<Billboard>("DamageText");
            damage.Initialize(_Damage.ToString(), pos);
            damage.gameObject.SetActive(true);
        }

        m_Health -= _Damage;
        if (m_Health <= 0.0f)
        {
            m_Live = E_Live.DEAD;
            m_Animator.CrossFade(m_AnimKeyDeath, 0.15f);
            SoundManager.Instance.PlayDefaultSound(m_DeathClip);
        }
        else
        {
            if (m_UseHitAnimation)
            {
                m_HitMotion = true;
                if (!_Knockback)
                    m_Animator.CrossFade(m_AnimKeyHit, 0.15f);
                else
                {
                    m_Down = true;
                    Vector3 dir = (_Attacker.transform.position - transform.position);
                    dir.y = 0.0f;
                    dir.Normalize();
                    transform.forward = dir;
                    m_Rigidbody.velocity = (-dir * Mathf.Clamp((_Damage * 1.25f), 0.0f, 10.0f)) + (Vector3.up * Mathf.Clamp(_Damage * 0.5f, 1.5f, 2.5f));
                    m_Animator.CrossFade(m_AnimKeyHitAirStart, 0.15f);
                    StartCoroutine(C_HitAirCollision());
                }
            }
        }
    }

    IEnumerator C_HitAirCollision()
    {
        int targetlayer = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Obstacle");
        while (true)
        {
            float distanceToPoint = m_CapsuleCollider.height / 2.0f - m_CapsuleCollider.radius;
            Vector3 point1 = transform.position + m_CapsuleCollider.center + (Vector3.up * distanceToPoint * 1.05f);
            Vector3 point2 = transform.position + m_CapsuleCollider.center - (Vector3.up * distanceToPoint * 1.05f);
            float radius = m_CapsuleCollider.radius * 0.95f;

            Collider[] colls = Physics.OverlapCapsule(point1, point2, radius, targetlayer);
            if (colls.Length > 0)
            {
                m_Animator.CrossFade(m_AnimKeyHitAirEnd, 0.15f);
                yield break;
            }
            yield return null;
        }
    }

    public void SetDeadState()
    {
        m_ActiveMotionRunning = false;
        m_IsDashing = false;
        m_IsDashing = false;
        m_Down = false;
    }

    public void StartNewMotion()
    {
        m_ActiveMotionRunning = true;
        m_MotionCancelDelay = 0.15f;
    }

    public bool CheckMotionCancelAvailability(int _DecreaseGauge = 2)
    {
        if (GameManager.Instacne.m_Main.IsPlayStop || GameManager.Instacne.m_Main.IsGameStop) return false;
        if (m_Live == E_Live.DEAD) return false;
        if (m_StopCharacter) return false;
        if (m_HitMotion) return false;
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

    public virtual void HitEnd(int _Value)
    {
        m_HitMotion = false;
        m_Down = false;
        MotionEnd(_Value);
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
