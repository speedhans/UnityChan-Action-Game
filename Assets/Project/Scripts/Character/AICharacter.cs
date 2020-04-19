using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICharacter : CharacterBase
{
    static readonly int m_AnimKeyIdleStand = Animator.StringToHash("Idle Non Loop");

    public struct S_TargetData
    {
        public Vector3 Direction;
        public Vector3 DirectionNormalize;
        public Vector3 DirectionNormalize2D;
        public float Distance;
        public float AngleBetweenTarget; // 2D 데이터를 기반으로 만듬 높이 값은 계산시 제외함
    }

    public CharacterBase m_TargetCharacter;
    S_TargetData m_TargetData;

    protected override void Awake()
    {
        base.Awake();
        m_IsAI = true;
    }

    protected override void Update()
    {
        if (m_TargetCharacter)
        {
            if (m_TargetCharacter.m_Live == E_Live.DEAD)
            {
                ResetState();
            }
            else
            {
                S_TargetData data;
                data.Direction = m_TargetCharacter.transform.position - transform.position;
                data.DirectionNormalize = data.Direction.normalized;
                data.DirectionNormalize2D = new Vector3(data.Direction.x, 0.0f, data.Direction.z).normalized;
                float dot = Vector3.Dot(transform.forward, data.DirectionNormalize2D);
                data.AngleBetweenTarget = (1.0f - ((dot + 1.0f) * 0.5f)) * 180.0f;
                data.Distance = data.Direction.magnitude;
                m_TargetData = data;
            }
        }

        base.Update();
    }

    public S_TargetData GetTargetData()
    {
        if (!m_TargetCharacter) return new S_TargetData();
        return m_TargetData;
    }

    public virtual void ResetState()
    {
        m_TargetCharacter = null;
        m_ActiveMotionRunning = false;
    }
}
