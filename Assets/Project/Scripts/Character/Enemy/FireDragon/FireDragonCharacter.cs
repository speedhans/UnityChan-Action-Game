using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonCharacter : AICharacter
{
    public Transform m_HeadPoint;
    public Transform[] m_TailPoint;

    protected override void Awake()
    {
        base.Awake();

        m_LeftHandPoint = FindBone(m_Animator.transform, "LeftHandPoint");
        m_RightHandPoint = FindBone(m_Animator.transform, "RightHandPoint");

        SetComponent<EnemySearchComponent>(this);
        SetComponent<FireDragonNormalAttackComponent>(this);
    }
}
