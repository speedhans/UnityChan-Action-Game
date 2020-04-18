using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonDashComponent : FireDragonBaseComponent
{
    readonly int m_AnimKeyDashLeftType = Animator.StringToHash("DashLeftType");
    readonly int m_AnimKeyDashRightType = Animator.StringToHash("DashRightType");

    public float m_Cooldown = 9.0f;
    public float m_CooldownTimer;

    public float m_Duration = 2.0f;
    public float m_DurationTimer;

    public float m_UseMaxDistance = 25.0f;
    bool m_UseDash = false;
    int m_DashLevel = 0;
    HashSet<CharacterBase> m_DamageList = new HashSet<CharacterBase>();

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_Frequency = 40;
        m_CharacterBase.m_AnimCallback.AddMotionStartEvent(MotionStart, 21);
        m_CharacterBase.m_AnimCallback.AddMotionEndEvent(MotionEnd, 21);
    }

    public override void FixedUpdateComponent(float _FixedDeltaTime)
    {
        base.FixedUpdateComponent(_FixedDeltaTime);
        if (m_DashLevel == 1)
            DashLevel1(_FixedDeltaTime);
        else if (m_DashLevel == 2)
            DashLevel2(_FixedDeltaTime);

        if (m_CooldownTimer > 0.0f)
        {
            m_CooldownTimer -= _FixedDeltaTime;
            return;
        }

        if (!DefaultStateCheck()) return;
        if (!DragonStateCheck()) return;
        if (!m_AICharacter.m_TargetCharacter) return;

        AICharacter.S_TargetData data = m_AICharacter.GetTargetData();
        if (data.AngleBetweenTarget < 90.0f && data.Distance > m_UseMaxDistance) return;
        if (!CalculateFrequency((int)(data.Distance - data.AngleBetweenTarget)))
        {
            m_CooldownTimer = m_Cooldown * 0.3f;
            return;
        }

        StartNewMotion();
        m_CooldownTimer = m_Cooldown;
        m_FireDragonCharacter.m_IsDashing = true;
        if (Vector3.Dot(m_AICharacter.transform.right, data.DirectionNormalize2D) > 0.0f)
            m_AICharacter.m_Animator.CrossFade(m_AnimKeyDashRightType, 0.15f);
        else
            m_AICharacter.m_Animator.CrossFade(m_AnimKeyDashLeftType, 0.15f);
        m_AICharacter.m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    void MotionStart()
    {
        m_UseDash = true;
        m_DamageList.Clear();
        m_DurationTimer = m_Duration;
        m_DashLevel = 1;
    }

    void MotionHalfEnd()
    {
        m_DamageList.Clear();
        m_CharacterBase.m_Animator.CrossFade(CharacterBase.m_AnimKeyIdle, 0.35f);
        m_FireDragonCharacter.m_ActiveMotionRunning = false;
        m_FireDragonCharacter.m_IsDashing = false;
        m_AICharacter.m_Rigidbody.velocity = m_AICharacter.transform.forward * 5.0f;
        m_DurationTimer = 0.5f;
        m_DashLevel = 2;
    }

    void MotionEnd()
    {
        m_UseDash = false;
        m_FireDragonCharacter.m_ActiveMotionRunning = false;
        m_FireDragonCharacter.m_IsDashing = false;
        m_AICharacter.m_Rigidbody.velocity = Vector3.zero;
        m_AICharacter.m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        m_DashLevel = 0;
    }

    void DashLevel1(float _DeltaTime)
    {
        if (!m_UseDash) return;

        if (m_DurationTimer > 0.0f)
        {
            m_DurationTimer -= _DeltaTime;
            if (m_DurationTimer <= 0.0f)
            {
                MotionHalfEnd();
                return;
            }
        }

        m_AICharacter.m_Rigidbody.velocity = m_AICharacter.transform.forward * 10.0f;
        Vector3 pos = m_FireDragonCharacter.transform.position + Vector3.up;
        HashSet<CharacterBase> hash = OverlabSphere(m_FireDragonCharacter, pos, 1.75f, ref m_DamageList);
        foreach (CharacterBase c in hash)
        {
            c.GiveToDamage(m_FireDragonCharacter, 5.0f, true);
            m_DamageList.Add(c);
        }
    }

    void DashLevel2(float _DeltaTime)
    {
        if (!m_UseDash) return;

        if (m_DurationTimer > 0.0f)
        {
            m_DurationTimer -= _DeltaTime;
            if (m_DurationTimer <= 0.0f)
            {
                MotionEnd();
                return;
            }
        }
    }
}
