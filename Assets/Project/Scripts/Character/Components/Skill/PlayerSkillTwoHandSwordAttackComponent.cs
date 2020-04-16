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
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(HitEvent, 12);
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

        if (CheckMotionCancelAvailability())
            SkillAnimationPlay(true, 0.25f);
        else if (!DefaultStateCheck()) return;
        else SkillAnimationPlay();
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
        if (HitDamage(m_CharacterBase, pos, 1.0f, 0.75f))
        {
            UIManager.Instacne.m_MotionCancelGauge.AddGauge(1);
        }
    }
}
