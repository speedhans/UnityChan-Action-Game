using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillFinishPunchComponent : PlayerSkillBaseComponent
{
    const string m_ChargeEffect = "FinishPunshEffect1";

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_AnimHash = Animator.StringToHash("FinishPunch1");
        m_HitEffectPrefabName = "FinishPunchHitEffect";

        InputManager.Instacne.AddNumberKeyEvent(5, FinishPunch);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(100, HitEvent);
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
        InputManager.Instacne.ReleaseNumberKeyEvent(5, FinishPunch);
    }
    void FinishPunch(InputActionPhase _Phase)
    {
        if (_Phase != InputActionPhase.Started) return;
        if (!CheckSkillAvailability()) return;
        if (!AnimStateCheck()) return;

        if (CheckMotionCancelAvailability())
        {
            m_PlayerCharacter.StartMotionCancelRim(5.0f, 0.75f);
            m_CharacterBase.m_Animator.CrossFade(m_AnimHash, 0.0f);
        }
        else if (!DefaultStateCheck()) return;
        m_PlayerCharacter.AllWeaponDisable();
        m_CharacterBase.m_Animator.CrossFade(m_AnimHash, 0.15f);
        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_ChargeEffect);
        life.Initialize();
        life.SetTargetTransform(m_CharacterBase.m_RightHandPoint);
        life.gameObject.SetActive(true);
        SoundManager.Instance.PlayDefaultSound(m_PlayerCharacter.m_FinishAttackClip1);
        StartNewMotion();

        m_PlayerCharacter.m_FinishCamera.SetFocus();
    }

    void HitEvent()
    {
        Vector3 pos = m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward);
        if (HitDamage(pos, 1.0f, 0.5f))
        {
            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_HitEffectPrefabName);
            if (life)
            {
                life.Initialize();
                life.transform.position = m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward * 0.35f);
                life.gameObject.SetActive(true);
            }
        }
    }
}
