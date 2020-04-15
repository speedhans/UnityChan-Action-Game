using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillBaseComponent : CharacterBaseComponent
{
    protected int m_AnimHash = 0;
    protected string m_HitEffectPrefabName;

    public float m_Cooldown;
    public float m_CooldownMax;

    protected System.Action m_DamageHitEvent;

    Ray m_Ray = new Ray();
    protected PlayerCharacter m_PlayerCharacter;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_PlayerCharacter = _CharacterBase as PlayerCharacter;
    }

    protected bool HitDamage(Vector3 _CollisionPoint, float _Damage, float _Radius)
    {
        Collider[] colls = Physics.OverlapSphere(_CollisionPoint, _Radius, m_CharacterBase.m_EnemyLayerMask);
        bool hit = false;
        for (int i = 0; i < colls.Length; ++i)
        {
            if (colls[i].gameObject == m_CharacterBase.gameObject) continue;
            CharacterBase character = colls[i].GetComponent<CharacterBase>();
            if (!character) continue;
            if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD) continue;
            if (m_CharacterBase.m_Team == character.m_Team) continue;
            character.GiveToDamage(m_CharacterBase.m_CharacterID, _Damage);
            hit = true;
        }

        if (hit)
        {
            m_DamageHitEvent?.Invoke();
            return true;
        }
        return false;
    }

    protected bool HitDamage(Vector3 _StartPoint, Vector3 _Direction, float _Damage, float _Radius, float _Distance)
    {
        m_Ray.origin = _StartPoint;
        m_Ray.direction = _Direction;
        bool hit = false;
        RaycastHit[] hits = Physics.SphereCastAll(m_Ray, _Radius, _Distance, m_CharacterBase.m_EnemyLayerMask);
        for (int i = 0; i < hits.Length; ++i)
        {
            if (hits[i].transform.gameObject == m_CharacterBase.gameObject) continue;
            CharacterBase character = hits[i].transform.GetComponent<CharacterBase>();
            if (!character) continue;
            if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD) continue;
            if (m_CharacterBase.m_Team == character.m_Team) continue;
            character.GiveToDamage(m_CharacterBase.m_CharacterID, _Damage);
            hit = true;
        }

        if (hit)
        {
            m_DamageHitEvent?.Invoke();
            return true;
        }
        return false;
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
    }
}
