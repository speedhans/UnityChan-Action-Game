using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonBaseComponent : EnemyBaseComponent
{
    protected FireDragonCharacter m_FireDragonCharacter;
    public int m_Frequency = 0;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_FireDragonCharacter = _CharacterBase as FireDragonCharacter;
    }

    public bool DragonStateCheck()
    {
        if (m_FireDragonCharacter.m_IsRotating) return false;
        if (m_FireDragonCharacter.m_IsAvoiding) return false;
        if (m_FireDragonCharacter.m_IsDashing) return false;
        if (m_CharacterBase.m_Rigidbody.velocity != Vector3.zero) return false;

        return true;
    }

    protected bool CalculateFrequency(int _Offset = 0)
    {
        return m_Frequency + _Offset > Random.Range(0, 100) ? true : false;
    }
}
