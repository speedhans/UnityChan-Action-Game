using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillBaseComponent : CharacterBaseComponent
{
    public Sprite m_Sprite;

    protected int m_AnimHash = 0;
    protected string m_HitEffectPrefabName;

    public float m_Cooldown;
    public float m_CooldownMax = 2;
    protected PlayerCharacter m_PlayerCharacter;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_PlayerCharacter = _CharacterBase as PlayerCharacter;
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        if (m_Cooldown > 0.0f)
        {
            m_Cooldown -= _DeltaTime;
        }
    }

    protected bool CheckSkillAvailability()
    {
        if (m_Cooldown > 0.0f) return false;

        return true;
    }

    protected void SkillAnimationPlay(bool _MotionCancel = false, float _AnimNormalizeOffset = 0.0f)
    {
        if (_MotionCancel)
        {
            m_PlayerCharacter.StartMotionCancelRim(5.0f, 0.75f);
            m_CharacterBase.m_Animator.CrossFade(m_AnimHash, 0.0f, 0, _AnimNormalizeOffset);
            m_PlayerCharacter.CreateSpectrumMesh(1.0f, true, GameManager.Instacne.m_Main.m_SpectrumMaterial);
        }
        else
        {
            m_CharacterBase.m_Animator.CrossFade(m_AnimHash, 0.15f);
        }

        m_Cooldown = m_CooldownMax;
    }
}
