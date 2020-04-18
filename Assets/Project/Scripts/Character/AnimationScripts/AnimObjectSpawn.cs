using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimObjectSpawn : StateMachineBehaviour
{
    public enum E_POINT
    {
        LEFTHAND,
        RIGHTHAND,
        ORIGIN,
    }
    [SerializeField]
    E_POINT m_SpawnPoint;
    [SerializeField]
    GameObject m_ObjectPrefab;
    [SerializeField]
    float m_PlayNormalizeFrame;
    bool m_Enable = false;
    float m_CurrentFrame;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Enable = true;
        m_CurrentFrame = 0.0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float frame = stateInfo.normalizedTime;
        if (frame > 1.0f)
        {
            frame = frame - Mathf.Floor(frame);
            if (frame < m_CurrentFrame)
                m_Enable = true;
        }
        m_CurrentFrame = frame;
        if (!m_Enable) return;
        if (m_CurrentFrame <= m_PlayNormalizeFrame) return;
        m_Enable = false;

        CharacterBase character = animator.GetComponentInParent<CharacterBase>();
        if (character)
        {
            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_ObjectPrefab.name);
            if (life)
            {
                life.Initialize();
                switch (m_SpawnPoint)
                {
                    case E_POINT.LEFTHAND:
                        life.transform.position = character.m_LeftHandPoint.position;
                        break;
                    case E_POINT.RIGHTHAND:
                        life.transform.position = character.m_RightHandPoint.position;
                        break;
                    case E_POINT.ORIGIN:
                        life.transform.position = character.transform.position;
                        break;
                    default:
                        break;
                }
                life.gameObject.SetActive(true);
            }
        }
    }
}
