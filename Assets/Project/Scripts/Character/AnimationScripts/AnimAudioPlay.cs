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
    bool m_Enable = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Enable = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!m_Enable) return;
        if (stateInfo.normalizedTime <= m_PlayNormalizeFrame) return;
        m_Enable = false;
        if (m_3D)
            SoundManager.Instance.Play3DSound(animator.transform.position, m_Clip, m_Volume);
        else
            SoundManager.Instance.PlayDefaultSound(m_Clip, m_Volume);
    }
}
