using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBaseComponent
{
    protected CharacterBase m_CharacterBase;
    public virtual void Initialize(CharacterBase _CharacterBase)
    {
        m_CharacterBase = _CharacterBase;
    }

    public virtual void UpdateComponent(float _DeltaTime)
    {

    }

    public virtual void FixedUpdateComponent(float _FixedDeltaTime)
    {

    }

    public virtual void LateUpdateComponent(float _DeltaTime)
    {

    }

    public virtual void DestoryComponent()
    {

    }

    static public bool HitDamage(CharacterBase _Attacker, Vector3 _CollisionPoint, float _Damage, float _Radius)
    {
        HashSet<CharacterBase> damagehash = new HashSet<CharacterBase>();
        Collider[] colls = Physics.OverlapSphere(_CollisionPoint, _Radius, _Attacker.m_EnemyLayerMask);
        for (int i = 0; i < colls.Length; ++i)
        {
            if (colls[i].gameObject == _Attacker.gameObject) continue;
            CharacterBase character = colls[i].GetComponent<CharacterBase>();
            if (!character) continue;
            if (_Attacker.m_Live == CharacterBase.E_Live.DEAD) continue;
            if (_Attacker.m_Team == character.m_Team) continue;
            damagehash.Add(character);
        }

        if (damagehash.Count > 0)
        {
            foreach (CharacterBase c in damagehash)
            {
                c.GiveToDamage(_Attacker.m_CharacterID, _Damage);
            }
            return true;
        }
        return false;
    }

    static public bool HitDamage(CharacterBase _Attacker, Vector3 _StartPoint, Vector3 _Direction, float _Damage, float _Radius, float _Distance)
    {
        bool hit = false;
        RaycastHit[] hits = Physics.SphereCastAll(_StartPoint, _Radius, _Direction, _Distance, _Attacker.m_EnemyLayerMask);
        for (int i = 0; i < hits.Length; ++i)
        {
            if (hits[i].transform.gameObject == _Attacker.gameObject) continue;
            CharacterBase character = hits[i].transform.GetComponent<CharacterBase>();
            if (!character) continue;
            if (_Attacker.m_Live == CharacterBase.E_Live.DEAD) continue;
            if (_Attacker.m_Team == character.m_Team) continue;
            character.GiveToDamage(_Attacker.m_CharacterID, _Damage);
            hit = true;
        }

        if (hit)
        {
            return true;
        }
        return false;
    }

    static public HashSet<CharacterBase> OverlabShape(CharacterBase _Attacker, Vector3 _StartPoint, Vector3 _Direction, float _HalfAngle, float _Distance)
    {
        HashSet<CharacterBase> hash = new HashSet<CharacterBase>();
        Collider[] colls = Physics.OverlapSphere(_StartPoint, _Distance, _Attacker.m_EnemyLayerMask);

        for (int i = 0; i < colls.Length; ++i)
        {
            if (colls[i].gameObject == _Attacker.gameObject) continue;
            CharacterBase character = colls[i].GetComponent<CharacterBase>();
            if (!character) continue;
            if (_Attacker.m_Live == CharacterBase.E_Live.DEAD) continue;
            if (_Attacker.m_Team == character.m_Team) continue;

            Vector3 dir = character.transform.position - _Attacker.transform.position;
            dir.y = 0.0f;
            dir.Normalize();
            float angle = (1.0f - (Vector3.Dot(_Direction, dir) + 1.0f * 0.5f)) * 180.0f;
            if (angle > _HalfAngle) continue;

            hash.Add(character);
        }

        return hash;
    }

    protected bool DefaultStateCheck()
    {
        if (GameManager.Instacne.m_Main.IsPlayStop() || GameManager.Instacne.m_Main.IsGameStop()) return false;
        if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD) return false;
        if (m_CharacterBase.m_HitMotion) return false;
        if (m_CharacterBase.m_ActiveMotionRunning) return false;

        return true;
    }

    protected bool CheckMotionCancelAvailability()
    {
        return m_CharacterBase.CheckMotionCancelAvailability();
    }

    protected void StartNewMotion()
    {
        m_CharacterBase.StartNewMotion();
        m_CharacterBase.m_Rigidbody.velocity = Vector3.zero;
    }
}
