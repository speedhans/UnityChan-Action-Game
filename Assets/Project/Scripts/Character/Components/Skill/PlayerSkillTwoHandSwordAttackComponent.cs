using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillTwoHandSwordAttackComponent : PlayerSkillBaseComponent
{
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_AnimHash = Animator.StringToHash("TwoHandSwordAttack1");
        m_HitEffectPrefabName = "TwoHandSwordHitEffect";

        InputManager.Instacne.AddNumberKeyEvent(2, TwoHandSwordAttack);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(12, HitEvent);
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
        InputManager.Instacne.ReleaseNumberKeyEvent(2, TwoHandSwordAttack);
    }

    void TwoHandSwordAttack(InputActionPhase _Phase)
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
        m_PlayerCharacter.ActivateOnlyOneWeapon(12);
        SoundManager.Instance.PlayDefaultSound(m_PlayerCharacter.m_SkillAttackClip3);
        StartNewMotion();
    }

    void HitEvent()
    {
        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_HitEffectPrefabName);
        if (life)
        {
            life.Initialize();
            life.transform.position = m_CharacterBase.transform.position + m_CharacterBase.transform.forward;
            life.gameObject.SetActive(true);
        }
        Vector3 pos = m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward);
        if (HitDamage(pos, 1.0f, 0.75f))
        {
            UIManager.Instacne.m_MotionCancelGauge.AddGauge(1);
        }
    }
}
