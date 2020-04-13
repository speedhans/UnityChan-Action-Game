using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashComponent : CharacterBaseComponent
{
    static public readonly int m_DashAnimKey = Animator.StringToHash("Dash Tree");
    static public readonly string m_DashAxisKeyX = "DashDirectionX";
    static public readonly string m_DashAxisKeyY = "DashDirectionY";

    public float m_DashSpeed = 10.0f;
    PlayerCharacter m_PlayerCharacter;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_PlayerCharacter = _CharacterBase as PlayerCharacter;
        InputManager.Instacne.AddKeyEvent("leftShift", Dash);
    }


    public override void DestoryComponent()
    {
        base.DestoryComponent();
        InputManager.Instacne.ReleaseKeyEvent("leftShift", Dash);
    }


    void Dash(InputActionPhase _Phase)
    {
        if (_Phase != InputActionPhase.Started) return;
        if (InputManager.Instacne.m_Move2D == Vector2.zero) return;
        if (m_CharacterBase.m_IsDashing) return;
        if (CheckMotionCancelAvailability())
        {
            m_PlayerCharacter.StartMotionCancelRim(5.0f, 0.5f);
            m_CharacterBase.m_Animator.CrossFade(m_DashAnimKey, 0.0f);
        }
        else if (!DefaultStateCheck()) return;

        if (m_CharacterBase.m_Animator.GetCurrentAnimatorStateInfo(0).shortNameHash == m_DashAnimKey)
            m_CharacterBase.m_Animator.CrossFade(m_DashAnimKey, 0.0f);
        else
            m_CharacterBase.m_Animator.CrossFade(m_DashAnimKey, 0.05f);

        m_CharacterBase.m_ActiveMotionRunning = true;
        m_CharacterBase.m_IsDashing = true;
        m_CharacterBase.m_Animator.SetFloat(m_DashAxisKeyX, InputManager.Instacne.m_Move2D.x);
        m_CharacterBase.m_Animator.SetFloat(m_DashAxisKeyY, InputManager.Instacne.m_Move2D.y);
        m_CharacterBase.m_Rigidbody.velocity = (m_CharacterBase.transform.rotation * new Vector3(InputManager.Instacne.m_Move2D.x, 0.0f, InputManager.Instacne.m_Move2D.y)) * m_DashSpeed;
    }
}
