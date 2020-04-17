using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonBreathComponent : FireDragonBaseComponent
{
    static readonly int m_AnimBreath = Animator.StringToHash("Breath");
    static readonly int m_AnimBreathSwipe = Animator.StringToHash("BreathSwipe");
    readonly string m_BreathFirePrefab = "BreathFire";
    readonly string m_BreathHitEffectPrefab = "BreathHitEffect";

    public float m_UseMinDistance = 7.0f;
    public float m_UseMaxDistance = 20.0f;

    public float m_Damage = 10.0f;
    public float m_Cooldown;
    public float m_CooldownTimer = 10.0f;

    bool m_EnableBreath = false;
    Transform m_CurrentBreathPoint;
    HashSet<CharacterBase> m_DamageHash = new HashSet<CharacterBase>();
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);

        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(BreathStart, 101, 102);
        m_CharacterBase.m_AnimCallback.AddMotionEndEvent(BreathEnd, 101, 102);
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        if (m_EnableBreath)
        {
            HitBreath();
        }
    }

    void HitBreath()
    {
        RaycastHit[] hits = Physics.SphereCastAll(m_CurrentBreathPoint.position, 0.5f, m_CurrentBreathPoint.forward, m_UseMaxDistance, m_FireDragonCharacter.m_EnemyLayerMask);
        foreach(RaycastHit h in hits)
        {
            CharacterBase character = h.transform.GetComponentInParent<CharacterBase>();
            if (!character) continue;
            if (character.m_Team == m_AICharacter.m_Team) continue;
            if (m_DamageHash.Contains(character)) continue;
            m_DamageHash.Add(character);
            character.GiveToDamage(m_FireDragonCharacter, m_Damage, true);

            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_BreathHitEffectPrefab);
            life.Initialize();
            life.transform.position = character.transform.position + Vector3.up;
            life.gameObject.SetActive(true);
        }
    }

    public override void FixedUpdateComponent(float _FixedDeltaTime)
    {
        base.FixedUpdateComponent(_FixedDeltaTime);

        if (m_Cooldown > 0.0f)
        {
            m_Cooldown -= _FixedDeltaTime;
            return;
        }
        if (!DefaultStateCheck()) return;
        if (!DragonStateCheck()) return;
        if (!m_AICharacter.m_TargetCharacter) return;
        AICharacter.S_TargetData data = m_AICharacter.GetTargetData();
        if (data.AngleBetweenTarget > 3.0f) return;
        if (data.Distance < m_UseMinDistance ||
            data.Distance > m_UseMaxDistance * 0.75) return;

        int r = Random.Range(0, 100);
        if (data.AngleBetweenTarget < 0.1f && r > 50)
        {
            m_CurrentBreathPoint = m_FireDragonCharacter.m_BreathPoint;
            m_CharacterBase.m_Animator.CrossFade(m_AnimBreath, 0.15f);
        }
        else
        {
            m_CurrentBreathPoint = m_FireDragonCharacter.m_BreathPoint2;
            m_CharacterBase.m_Animator.CrossFade(m_AnimBreathSwipe, 0.15f);
        }
        m_Cooldown = m_CooldownTimer;
        StartNewMotion();
    }

    void BreathStart()
    {
        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_BreathFirePrefab);
        life.Initialize();
        life.transform.SetParent(m_CurrentBreathPoint);
        sunTT.sunTTHelper.SetLocalTransformIdentity(life.transform);
        life.gameObject.SetActive(true);
        m_DamageHash.Clear();
        m_EnableBreath = true;
    }

    public void BreathEnd()
    {
        m_DamageHash.Clear();
        m_EnableBreath = false;
    }
}
