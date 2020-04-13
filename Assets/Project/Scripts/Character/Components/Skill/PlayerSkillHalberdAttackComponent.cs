﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillHalberdAttackComponent : PlayerSkillBaseComponent
{
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_AnimHash = Animator.StringToHash("TwoHandHalberdAttack1");
        m_HitEffectPrefabName = "HalbertHitEffect";

        InputManager.Instacne.AddNumberKeyEvent(3, TwoHandHalberdAttack);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(13, HitEvent);
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
        InputManager.Instacne.ReleaseNumberKeyEvent(3, TwoHandHalberdAttack);
    }

    void TwoHandHalberdAttack(InputActionPhase _Phase)
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
        m_CharacterBase.m_Animator.CrossFade(m_AnimHash, 0.15f);
        m_PlayerCharacter.ActivateOnlyOneWeapon(13);
        SoundManager.Instance.PlayDefaultSound(m_PlayerCharacter.m_SkillAttackClip2);
        StartNewMotion();
    }

    void HitEvent()
    {
        Vector3 pos = m_CharacterBase.transform.position + Vector3.up;
        if (HitDamage(pos, m_PlayerCharacter.transform.forward, 1.0f, 0.5f, 1.0f))
        {
            UIManager.Instacne.m_MotionCancelGauge.AddGauge(1);

            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_HitEffectPrefabName);
            if (life)
            {
                life.Initialize();
                life.transform.SetPositionAndRotation(m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward * 0.35f), m_CharacterBase.transform.rotation);
                life.gameObject.SetActive(true);
            }
        }
    }
}