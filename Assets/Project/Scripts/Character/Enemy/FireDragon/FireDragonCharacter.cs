using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonCharacter : AICharacter
{
    public AudioClip m_RoarClip;

    public Transform m_HeadPoint;
    public Transform m_BreathPoint;
    public Transform m_BreathPoint2;
    public Transform[] m_TailPoint;

    [HideInInspector]
    public bool m_IsRotating = false;
    [HideInInspector]
    public bool m_IsAvoiding = false;

    protected override void Awake()
    {
        base.Awake();

        m_LeftHandPoint = FindBone(m_Animator.transform, "LeftHandPoint");
        m_RightHandPoint = FindBone(m_Animator.transform, "RightHandPoint");

        SetComponent<EnemySearchComponent>(this);
        SetComponent<FireDragonAvoidComponent>(this);
        SetComponent<FireDragonRotateComponent>(this);
        SetComponent<FireDragonNormalAttackComponent>(this);
        SetComponent<FireDragonBreathComponent>(this);
        SetComponent<FireDragonDashComponent>(this);
        SetComponent<FireDragonMiniMeteorComponent>(this);
    }

    public override void ResetState()
    {
        base.ResetState();
        m_IsRotating = false;
        m_IsAvoiding = false;
        m_IsDashing = false;
    }
}
