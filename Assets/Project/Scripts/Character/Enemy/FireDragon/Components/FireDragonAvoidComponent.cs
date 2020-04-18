using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonAvoidComponent : FireDragonBaseComponent
{
    static readonly int m_AnimAvoidLeft = Animator.StringToHash("AvoidLeft");
    static readonly int m_AnimAvoidRight = Animator.StringToHash("AvoidRight");

    float m_AvoidMaxDistance = 10.0f;
    float m_AvoidCooldown = 7.0f;
    float m_AvoidCooldownTimer;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_Frequency = 40;
        m_CharacterBase.m_AnimCallback.AddMotionEndEvent(AvoidEnd, 51,52);
    }

    public override void FixedUpdateComponent(float _DeltaTime)
    {
        base.FixedUpdateComponent(_DeltaTime);

        if (m_AvoidCooldownTimer > 0.0f)
        {
            m_AvoidCooldownTimer -= _DeltaTime;
            return;
        }
        
        if (!DefaultStateCheck()) return;
        if (!DragonStateCheck()) return;
        if (!m_AICharacter.m_TargetCharacter) return;
        if (m_AICharacter.GetTargetData().Distance > m_AvoidMaxDistance) return;
        if (!CalculateFrequency())
        {
            m_AvoidCooldownTimer = m_AvoidCooldown * 0.3f;
            return;
        }

        StartNewMotion();
        m_AvoidCooldownTimer = m_AvoidCooldown;
        m_FireDragonCharacter.m_IsAvoiding = true;
        //m_AICharacter.m_Animator.applyRootMotion = true;
        m_AICharacter.m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        float rightDot = Vector3.Dot(m_AICharacter.transform.right, m_AICharacter.GetTargetData().DirectionNormalize2D);
        float pow = Random.Range(0, 100) > 50 ? 15.0f : 10.0f;
        if (rightDot > 0.0f)
        {
            m_AICharacter.m_Rigidbody.velocity = (-m_AICharacter.transform.right * pow) + (Vector3.up * 2.0f);
            m_AICharacter.m_Animator.CrossFade(m_AnimAvoidLeft, 0.15f);
        }
        else
        {
            m_AICharacter.m_Rigidbody.velocity = (m_AICharacter.transform.right * pow) + (Vector3.up * 2.0f);
            m_AICharacter.m_Animator.CrossFade(m_AnimAvoidRight, 0.15f);
        }
    }

    void AvoidEnd()
    {
        m_AICharacter.m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        //m_AICharacter.m_Animator.applyRootMotion = false;
        m_FireDragonCharacter.m_IsAvoiding = false;
    }
}
