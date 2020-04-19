using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillSwordAttackComponent : PlayerSkillBaseComponent
{
    public float m_Damage = 5.0f;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_AnimHash = Animator.StringToHash("SwordAttack1");
        m_HitEffectPrefabName = "SwordHitEffect";
        m_Sprite = Resources.Load<Sprite>("UI/Sword1");
        InputManager.Instacne.AddNumberKeyEvent(1, SwordAttack);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(HitEvent, 11);

        UIManager.Instacne.m_SkillGroupUI.SetSkill(this, 0);
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
        InputManager.Instacne.ReleaseNumberKeyEvent(1, SwordAttack);
    }
    void SwordAttack(InputActionPhase _Phase)
    {
        if (_Phase != InputActionPhase.Started) return;
        if (!CheckSkillAvailability()) return;

        if (CheckMotionCancelAvailability())
            SkillAnimationPlay(true, 0.15f);
        else if (!DefaultStateCheck()) return;
        else SkillAnimationPlay();
        m_PlayerCharacter.ActivateOnlyOneWeapon(11);
        SoundManager.Instance.PlayDefaultSound(m_PlayerCharacter.m_SkillAttackClip1);
        StartNewMotion();
        m_PlayerCharacter.m_Rigidbody.velocity = m_PlayerCharacter.transform.forward * 10;
    }

    void HitEvent()
    {
        m_PlayerCharacter.m_Rigidbody.velocity = Vector3.zero;
        Vector3 pos = m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward);
        if (HitDamage(m_CharacterBase, pos, m_Damage, 0.5f))
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
