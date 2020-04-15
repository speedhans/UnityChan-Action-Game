using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySearchComponent : EnemyBaseComponent
{
    public float m_Radius = 10.0f;

    public override void FixedUpdateComponent(float _DeltaTime)
    {
        base.FixedUpdateComponent(_DeltaTime);
        if (!DefaultStateCheck()) return;
        if (m_AICharacter.m_TargetCharacter) return;

        CharacterBase cb = FindTarget();
        if (cb)
        {
            m_AICharacter.m_TargetCharacter = cb;
        }
    }

    CharacterBase FindTarget()
    {
        CharacterBase character = null;

        Collider[] colls = Physics.OverlapSphere(m_AICharacter.transform.position, m_Radius, m_AICharacter.m_EnemyLayerMask);
        float distacne = 9999999.0f;
        for (int i = 0; i < colls.Length; ++i)
        {
            float dist = Vector3.Distance(colls[i].transform.position, m_AICharacter.transform.position);
            if (dist < distacne)
            {
                CharacterBase cb = colls[i].GetComponentInParent<CharacterBase>();
                if (cb)
                {
                    character = cb;
                    dist = distacne;
                }
            }
        }

        return character;
    }
}
