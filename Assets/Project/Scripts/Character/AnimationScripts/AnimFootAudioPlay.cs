using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimFootAudioPlay : StateMachineBehaviour
{
    [SerializeField]
    AudioClip m_Clip;
    [SerializeField]
    float m_Volume = 1.0f;
    [SerializeField]
    bool m_3D = false;
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

        if (m_3D)
            SoundManager.Instance.Play3DSound(animator.transform.position, m_Clip == null ? GameManager.Instacne.m_Main.m_FootClip1 : m_Clip, m_Volume);
        else
            SoundManager.Instance.PlayDefaultSound(m_Clip == null ? GameManager.Instacne.m_Main.m_FootClip1 : m_Clip, m_Volume);
    }
}
