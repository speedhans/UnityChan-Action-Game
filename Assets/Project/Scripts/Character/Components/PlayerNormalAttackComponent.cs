using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNormalAttackComponent : CharacterBaseComponent
{
    static public readonly int m_AnimKeyAttack1 = Animator.StringToHash("RightAttack1");
    static public readonly int m_AnimKeyAttack2 = Animator.StringToHash("LeftAttack1");
    static public readonly int m_AnimKeyAttack3 = Animator.StringToHash("RightAttack2");

    static public readonly string m_EffectPunchPrefab = "AttackHit_white01";
    static public readonly string m_EffectKickPrefab = "AttackHit_white02";

    public int m_AttackLevel = 0;
    float m_Timer = 0.0f;

    PlayerCharacter m_PlayerCharacter;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);

        m_PlayerCharacter = _CharacterBase as PlayerCharacter;

        InputManager.Instacne.AddMouseEvent(0, MouseEvent);

        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(HitEvent1, 1);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(HitEvent2, 2);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(HitEvent3, 3);
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        if (m_Timer > 0.0f)
        {
            m_Timer -= _DeltaTime;
            if (m_Timer <= 0.0f)
            {
                m_AttackLevel = 0;
            }
        }
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
        InputManager.Instacne.ReleaseMouseEvent(0, MouseEvent);

        m_CharacterBase.m_AnimCallback.ReleaseAttackHitEvent(1);
        m_CharacterBase.m_AnimCallback.ReleaseAttackHitEvent(2);
        m_CharacterBase.m_AnimCallback.ReleaseAttackHitEvent(3);
    }

    void MouseEvent(InputActionPhase _Phase)
    {
        if (_Phase != InputActionPhase.Started) return;
        if (!DefaultStateCheck()) return;

        StartNewMotion();
        m_PlayerCharacter.AllWeaponDisable();
        switch (m_AttackLevel)
        {
            case 0:
                m_CharacterBase.m_Animator.CrossFade(m_AnimKeyAttack1, 0.15f);
                SoundManager.Instance.PlayDefaultSound(m_PlayerCharacter.m_NormalAttackClip, 0.5f);
                SoundManager.Instance.PlayDefaultSound(m_CharacterBase.m_AudioList[0], 0.5f);
                break;
            case 1:
                SoundManager.Instance.PlayDefaultSound(m_PlayerCharacter.m_NormalAttackClip, 0.5f);
                m_CharacterBase.m_Animator.CrossFade(m_AnimKeyAttack2, 0.15f);
                SoundManager.Instance.PlayDefaultSound(m_CharacterBase.m_AudioList[0], 0.5f);
                break;
            case 2:
                SoundManager.Instance.PlayDefaultSound(m_PlayerCharacter.m_NormalAttackClip, 0.5f);
                m_CharacterBase.m_Animator.CrossFade(m_AnimKeyAttack3, 0.1f);
                SoundManager.Instance.PlayDefaultSound(m_CharacterBase.m_AudioList[0], 0.5f);
                break;
        }
        m_AttackLevel = (m_AttackLevel + 1) % 3;
        m_Timer = 1.25f;
    }

    void HitEvent1()
    {
        Vector3 pos = m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward);
        if (HitDamage(m_CharacterBase, pos, new Vector3(0.35f, 1.0f, 0.4f), m_CharacterBase.transform.rotation, 1.0f))
        {
            HitEffect(m_EffectPunchPrefab, m_CharacterBase.m_RightHandPoint.position);
            m_PlayerCharacter.AddStemina(5);
        }
    }

    void HitEvent2()
    {
        Vector3 pos = m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward);
        if (HitDamage(m_CharacterBase, pos, new Vector3(0.35f, 1.0f, 0.4f), m_CharacterBase.transform.rotation, 1.0f))
        {
            HitEffect(m_EffectPunchPrefab, m_CharacterBase.m_LeftHandPoint.position);
            m_PlayerCharacter.AddStemina(7);
        }
    }

    void HitEvent3()
    {
        Vector3 pos = m_CharacterBase.transform.position + (Vector3.up + m_CharacterBase.transform.forward);
        if (HitDamage(m_CharacterBase, pos, new Vector3(0.35f, 1.0f, 0.4f), m_CharacterBase.transform.rotation, 1.0f))//if (HitDamage(m_CharacterBase, pos, 1.0f, 0.45f))
        {
            HitEffect(m_EffectKickPrefab, pos);
            m_PlayerCharacter.AddStemina(10);
        }
    }

    void HitEffect(string _EffectPath, Vector3 _EffectPosition)
    {
        UIManager.Instacne.m_MotionCancelGauge.AddGauge(1);
        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(_EffectPath);
        if (life)
        {
            life.Initialize();
            life.transform.position = _EffectPosition;
            life.gameObject.SetActive(true);
        }
    }
}
