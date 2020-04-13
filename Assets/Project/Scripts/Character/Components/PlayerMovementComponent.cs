using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementComponent : CharacterBaseComponent
{
    public float m_MovementSpeed = 5.0f;
    public float m_AccelerationSpeed = 15.0f;
    public float m_CurrentSpeed = 0.0f;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
    }

    public override void FixedUpdateComponent(float _FixedDeltaTime)
    {
        if (!DefaultStateCheck()) return;

        base.FixedUpdateComponent(_FixedDeltaTime);
        if (InputManager.Instacne.m_Move2D == Vector2.zero)
            m_CurrentSpeed -= m_AccelerationSpeed * _FixedDeltaTime * 2.0f;
        else
            m_CurrentSpeed += m_AccelerationSpeed * _FixedDeltaTime;
        m_CurrentSpeed = Mathf.Clamp(m_CurrentSpeed, 0.0f, m_MovementSpeed);

        Vector3 speed = new Vector3(InputManager.Instacne.m_Move2D.x, 0.0f, InputManager.Instacne.m_Move2D.y) * m_CurrentSpeed;

        m_CharacterBase.m_Animator.SetFloat(CharacterBase.m_AnimKeyMoveDirectionX, speed.x);
        m_CharacterBase.m_Animator.SetFloat(CharacterBase.m_AnimKeyMoveDirectionY, speed.z);
        m_CharacterBase.m_Rigidbody.velocity = m_CharacterBase.transform.rotation * speed;
    }

    public override void LateUpdateComponent(float _DeltaTime)
    {
        base.LateUpdateComponent(_DeltaTime);
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
    }


}
