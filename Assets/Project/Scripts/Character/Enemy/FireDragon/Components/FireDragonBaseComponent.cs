using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonBaseComponent : EnemyBaseComponent
{
    static Vector3 StopVector3 = new Vector3(0.001f, 0.001f, 0.001f);

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
        Vector3 v = sunTT.sunTTHelper.AbsVector3(m_CharacterBase.m_Rigidbody.velocity);
        if (v.x > StopVector3.x ||
            v.y > StopVector3.y ||
            v.z > StopVector3.z) return false;

        return true;
    }

    protected bool CalculateFrequency(int _Offset = 0)
    {
        return m_Frequency + _Offset > Random.Range(0, 100) ? true : false;
    }

    protected bool ConditionCheck()
    {
        if (GameManager.Instacne.m_GameLevel == 1 && (m_FireDragonCharacter.m_Health / m_FireDragonCharacter.m_HealthMax) > 0.5f) return false;

        return true;
    }
}
