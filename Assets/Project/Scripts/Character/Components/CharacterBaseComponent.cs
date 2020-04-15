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
