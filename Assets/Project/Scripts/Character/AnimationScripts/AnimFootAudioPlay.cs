using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFootAudioPlay : StateMachineBehaviour
{
    [SerializeField]
    float m_Volume = 1.0f;
    [SerializeField]
    float m_PlayNormalizeFrame;
    float m_CurrentFrame = 0.0f;
    bool m_Enable = false;

    static readonly int m_AnimKeyMoveDirectionX = Animator.StringToHash("MoveDirectionX");
    static readonly int m_AnimKeyMoveDirectionY = Animator.StringToHash("MoveDirectionY");
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float normalizeTime = stateInfo.normalizedTime;
        normalizeTime = normalizeTime - Mathf.Floor(normalizeTime);
        if (normalizeTime < m_CurrentFrame)
        {
            m_Enable = true;
            m_CurrentFrame = normalizeTime;
        }

        if (!m_Enable) return;
        float x = Mathf.Abs(animator.GetFloat(m_AnimKeyMoveDirectionX));
        float y = Mathf.Abs(animator.GetFloat(m_AnimKeyMoveDirectionY));
        if (x < 0.3f && y < 0.3f) return;
        if (normalizeTime <= m_PlayNormalizeFrame) return;
        m_Enable = false;
        Debug.Log("Foot");
        SoundManager.Instance.PlayDefaultSound(GameManager.Instacne.m_Main.m_FootClip1, m_Volume);
    }
}
