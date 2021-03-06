﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillFinishPunchComponent : PlayerSkillBaseComponent
{
    const string m_ChargeEffect = "FinishPunshEffect1";
    public float m_Damage = 10.0f;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_AnimHash = Animator.StringToHash("FinishPunch1");
        m_HitEffectPrefabName = "FinishPunchHitEffect";
        m_Sprite = Resources.Load<Sprite>("UI/FinishPunch");

        InputManager.Instacne.AddNumberKeyEvent(5, FinishPunch);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(HitEvent, 100);
        m_CharacterBase.m_AnimCallback.AddMotionEndEvent(FinishPunchEnd, 100);
        m_CooldownMax = 5.0f;
        UIManager.Instacne.m_SkillGroupUI.SetSkill(this, 4);
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
        InputManager.Instacne.ReleaseNumberKeyEvent(5, FinishPunch);
    }
    void FinishPunch(InputActionPhase _Phase)
    {
        if (_Phase != InputActionPhase.Started) return;
        if (UIManager.Instacne.m_ComboViewer.GetCurrentCombo() < 7) return;
        if (!CheckSkillAvailability()) return;

        if (CheckMotionCancelAvailability())
            SkillAnimationPlay(true, 0.5f);
        else if (!DefaultStateCheck()) return;
        else SkillAnimationPlay();
        m_PlayerCharacter.AllWeaponDisable();
        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_ChargeEffect);
        life.Initialize();
        life.SetTargetTransform(m_CharacterBase.m_RightHandPoint);
        life.gameObject.SetActive(true);
        SoundManager.Instance.PlayDefaultSound(m_PlayerCharacter.m_FinishAttackClip1);
        StartNewMotion();

        m_PlayerCharacter.m_Immortal = true;
        m_PlayerCharacter.m_StopCharacter = true;
        m_PlayerCharacter.m_FinishCamera.SetFocus();
    }

    void HitEvent()
    {
        Vector3 pos = m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward);
        if (HitDamage(m_CharacterBase, pos, Mathf.Floor((m_Damage + (UIManager.Instacne.m_ComboViewer.GetCurrentCombo() * 1.2f))), 0.7f))
        {
            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_HitEffectPrefabName);
            if (life)
            {
                life.Initialize();
                life.transform.SetPositionAndRotation(m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward * 0.35f), m_CharacterBase.transform.rotation);
                life.gameObject.SetActive(true);
            }
        }
    }

    void FinishPunchEnd()
    {
        m_PlayerCharacter.m_Immortal = false;
        m_PlayerCharacter.m_StopCharacter = false;
    }
}
