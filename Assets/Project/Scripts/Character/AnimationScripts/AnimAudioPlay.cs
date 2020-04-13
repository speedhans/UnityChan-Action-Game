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
        SoundManager.Instance.PlayDefaultSound(m_Clip, m_Volume);
    }
}
