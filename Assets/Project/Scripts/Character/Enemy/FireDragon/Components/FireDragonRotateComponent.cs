using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonRotateComponent : FireDragonBaseComponent
{
    static int m_AnimKeyTurnLeft = Animator.StringToHash("TurnLeft");
    static int m_AnimKeyTurnRight = Animator.StringToHash("TurnRight");

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
    }

    public override void FixedUpdateComponent(float _FixedDeltaTime)
    {
        base.FixedUpdateComponent(_FixedDeltaTime);
        if (!m_AICharacter.m_TargetCharacter) return;
        if (!DefaultStateCheck()) return;
        if (m_FireDragonCharacter.m_IsAvoiding) return;

        float angle = m_AICharacter.GetTargetData().AngleBetweenTarget;

        if (!m_FireDragonCharacter.m_IsRotating)
        {
            if (angle > 2.0f)
            {
                m_FireDragonCharacter.m_IsRotating = true;
                m_AICharacter.m_Animator.applyRootMotion = true;
                float rdot = Vector3.Dot(m_AICharacter.transform.right, m_AICharacter.GetTargetData().DirectionNormalize2D);
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
        else if (angle < 0.1f)
        {
            if (m_AICharacter.m_Animator.applyRootMotion)
            {
                m_AICharacter.m_Animator.applyRootMotion = false;
                m_AICharacter.m_Animator.CrossFade(CharacterBase.m_AnimKeyIdle, 0.15f);
                m_FireDragonCharacter.m_IsRotating = false;
                m_FireDragonCharacter.transform.forward = m_AICharacter.GetTargetData().DirectionNormalize2D;
            }
        }
    }
}
