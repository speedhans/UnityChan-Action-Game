using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonBaseComponent : EnemyBaseComponent
{
    protected FireDragonCharacter m_FireDragonCharacter;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_FireDragonCharacter = _CharacterBase as FireDragonCharacter;
    }

    public bool DragonStateCheck()
    {
        if (m_FireDragonCharacter.m_IsRotating) return false;

        return true;
    }
}
