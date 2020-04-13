using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillKatanaAttackComponent : PlayerSkillBaseComponent
{
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_AnimHash = Animator.StringToHash("KatanaAttack1");
        m_HitEffectPrefabName = "KatanaHitEffect";

        InputManager.Instacne.AddNumberKeyEvent(4, KatanaAttack);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(14, HitEvent);
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
        InputManager.Instacne.ReleaseNumberKeyEvent(4, KatanaAttack);
    }
    void KatanaAttack(InputActionPhase _Phase)
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
        m_PlayerCharacter.ActivateOnlyOneWeapon(14);
        SoundManager.Instance.PlayDefaultSound(m_PlayerCharacter.m_SkillAttackClip2);
        StartNewMotion();
    }

    void HitEvent()
    {
        Vector3 pos = m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward);
        if (HitDamage(pos, 1.0f, 0.5f))
        {
            UIManager.Instacne.m_MotionCancelGauge.AddGauge(1);

            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_HitEffectPrefabName);
            if (life)
            {
                life.Initialize();
                life.transform.SetPositionAndRotation(m_CharacterBase.transform.position + (Vector3.up * 0.65f + m_CharacterBase.transform.forward * 0.35f), m_CharacterBase.transform.rotation);
                life.gameObject.SetActive(true);
            }
        }
    }
}
