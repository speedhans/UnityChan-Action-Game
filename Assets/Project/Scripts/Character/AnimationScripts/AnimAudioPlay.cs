using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimAudioPlay : StateMachineBehaviour
{
    [SerializeField]
    AudioClip m_Clip;
    [SerializeField]
    float m_Volume = 1.0f;
    [SerializeField]
    bool m_3D = false;
    [SerializeField]
    float m_PlayNormalizeFrame;
    float m_CurrentFrame;
    bool m_Enable = false;
    [SerializeField]
    bool m_Loop = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Enable = true;
        m_CurrentFrame = 0.0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float frame = stateInfo.normalizedTime;
        if (m_Loop && frame > 1.0f)
        {
            frame -= Mathf.Floor(frame);
            if (frame < m_CurrentFrame)
                m_Enable = true;
        }
        m_CurrentFrame = frame;
        if (!m_Enable) return;
        if (m_CurrentFrame <= m_PlayNormalizeFrame) return;
        m_Enable = false;

        if (m_3D)
            SoundManager.Instance.Play3DSound(animator.transform.position, m_Clip, m_Volume);
        else
            SoundManager.Instance.PlayDefaultSound(m_Clip, m_Volume);
    }
}
