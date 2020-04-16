using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonRotateComponent : FireDragonBaseComponent
{
    static int m_AnimKeyTurnLeft = Animator.StringToHash("TurnLeft");
    static int m_AnimKeyTurnRight = Animator.StringToHash("TurnRight");

    public float m_RotateSpeed = 180.0f;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
    }

    public override void FixedUpdateComponent(float _FixedDeltaTime)
    {
        base.FixedUpdateComponent(_FixedDeltaTime);
        if (!m_AICharacter.m_TargetCharacter) return;
        if (!DefaultStateCheck()) return;

        Vector3 dir = m_AICharacter.m_TargetCharacter.transform.position - m_AICharacter.transform.position;
        dir.y = 0.0f;
        dir.Normalize();
        float dot = Vector3.Dot(m_AICharacter.transform.forward, dir);
        float angle = (1.0f - ((dot + 1.0f) * 0.5f)) * 180.0f;

        if (!m_FireDragonCharacter.m_IsRotating)
        {
            if (angle > 5.0f)
            {
                m_FireDragonCharacter.m_IsRotating = true;
                m_AICharacter.m_Animator.applyRootMotion = true;
                float rdot = Vector3.Dot(m_AICharacter.transform.right, dir);
                AnimatorStateInfo info = m_AICharacter.m_Animator.GetCurrentAnimatorStateInfo(0);
                if (rdot > 0.0f)
                {
                    m_AICharacter.m_Animator.CrossFade(m_AnimKeyTurnRight, 0.15f);
                }
                else
                {
                    m_AICharacter.m_Animator.CrossFade(m_AnimKeyTurnLeft, 0.15f);
                }
            }
        }
        else if (angle < 0.5f)
        {
            if (m_AICharacter.m_Animator.applyRootMotion)
            {
                m_AICharacter.m_Animator.applyRootMotion = false;
                m_AICharacter.m_Animator.CrossFade(CharacterBase.m_AnimKeyIdle, 0.15f);
                m_FireDragonCharacter.m_IsRotating = false;
            }
        }
    }
}
