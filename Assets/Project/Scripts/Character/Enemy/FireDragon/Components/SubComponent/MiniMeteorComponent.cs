using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMeteorComponent : MonoBehaviour
{
    Ray m_Ray = new Ray();
    RaycastHit m_RayHit;

    CharacterBase m_Other;
    Vector3 m_Direction;
    float m_Speed;
    float m_Damage;
    float m_DamageRadius;

    [SerializeField]
    GameObject m_MiniMeteorImpactPrefab;

    int m_GroundLayer;

    bool m_Hit = false;
    private void Awake()
    {
        m_GroundLayer = 1 << LayerMask.NameToLayer("Ground") | 1 << LayerMask.NameToLayer("Obstacle");
    }

    public void Initialize(CharacterBase _Attacker, Vector3 _Direction, float _Speed, float _Damage, float _DamageRadius)
    {
        m_Other = _Attacker;
        m_Direction = _Direction.normalized;
        m_Speed = _Speed;
        m_Damage = _Damage;
        m_DamageRadius = _DamageRadius;
        m_Hit = false;
    }

    private void Update()
    {
        float deltatime = Time.deltaTime;

        m_Ray.origin = transform.position;
        m_Ray.direction = m_Direction;

        if (!m_Hit && Physics.SphereCast(m_Ray, 0.5f, out m_RayHit, m_Speed* deltatime, m_GroundLayer))
        {
            m_Hit = true;
            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_MiniMeteorImpactPrefab.name);
            life.Initialize();
            life.transform.position = m_RayHit.point;
            life.gameObject.SetActive(true);

            RaycastHit[] hits = Physics.SphereCastAll(m_Ray, m_DamageRadius, m_Speed * deltatime, m_Other.m_EnemyLayerMask);
            foreach(RaycastHit h in hits)
            {
                CharacterBase character = h.transform.GetComponentInParent<CharacterBase>();
                if (!character) continue;
                if (character.m_Live == CharacterBase.E_Live.DEAD) continue;
                if (character.m_Team == m_Other.m_Team) continue;

                character.GiveToDamage(m_Other, m_Damage, true);
            }

            LifeTimerWithObjectPool mylife = GetComponent<LifeTimerWithObjectPool>();
            mylife.SetTimer(1.5f);
        }
        else
        {
            Vector3 nextpos = transform.position + m_Direction * m_Speed * deltatime;
            transform.position = nextpos;
        }
    }
}
