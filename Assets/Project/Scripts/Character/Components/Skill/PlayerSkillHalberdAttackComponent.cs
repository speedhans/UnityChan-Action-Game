using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSkillHalberdAttackComponent : PlayerSkillBaseComponent
{
    public float m_Damage = 10.0f;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_AnimHash = Animator.StringToHash("TwoHandHalberdAttack1");
        m_HitEffectPrefabName = "HalbertHitEffect";
        m_Sprite = Resources.Load<Sprite>("UI/WarAxe");
        InputManager.Instacne.AddNumberKeyEvent(3, TwoHandHalberdAttack);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(HitEvent, 13);

        UIManager.Instacne.m_SkillGroupUI.SetSkill(this, 2);
    }

    void TwoHandHalberdAttack(InputActionPhase _Phase)
    {
        if (_Phase != InputActionPhase.Started) return;
        if (!CheckSkillAvailability()) return;

        if (CheckMotionCancelAvailability())
            SkillAnimationPlay(true, 0.7f);
        else if (!DefaultStateCheck()) return;
        else SkillAnimationPlay();
        m_PlayerCharacter.ActivateOnlyOneWeapon(13);
        SoundManager.Instance.PlayDefaultSound(m_PlayerCharacter.m_SkillAttackClip2);
        StartNewMotion();
    }

    void HitEvent()
    {
        Vector3 pos = m_CharacterBase.transform.position + Vector3.up;
        if (HitDamage(m_CharacterBase, pos, m_PlayerCharacter.transform.forward, m_Damage, 0.5f, 1.5f))
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
