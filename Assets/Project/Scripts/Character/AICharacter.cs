using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : CharacterBase
{
    public CharacterBase m_TargetCharacter;

    protected override void Awake()
    {
        base.Awake();
        m_IsAI = true;
    }
}
