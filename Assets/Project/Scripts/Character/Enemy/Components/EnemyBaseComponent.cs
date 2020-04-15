using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseComponent : CharacterBaseComponent
{
    protected AICharacter m_AICharacter;
    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_AICharacter = _CharacterBase as AICharacter;
    }
}
