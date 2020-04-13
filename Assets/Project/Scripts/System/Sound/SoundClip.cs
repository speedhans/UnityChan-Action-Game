using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundClip : MonoBehaviour
{
    [SerializeField]
    AudioClip[] m_Clip;
    [SerializeField]
    float m_Volume = 1.0f;
    [SerializeField]
    bool m_3D = false;
    [SerializeField]
    bool m_Loop = false;
    [SerializeField]
    bool m_AutoPlay = true;

    [SerializeField]
    bool m_NoFirstRun = false;
    private void OnEnable()
    {
        if (!m_AutoPlay) return;
        if (m_NoFirstRun)
        {
            m_NoFirstRun = false;
            return;
        }
        if (m_Clip == null || m_Clip.Length < 0) return;
        if (m_3D)
            SoundManager.Instance.Play3DSound(transform.position, m_Clip[Random.Range(0, m_Clip.Length - 1)], m_Volume);
        else
            SoundManager.Instance.PlayDefaultSound(m_Clip[Random.Range(0, m_Clip.Length - 1)], m_Volume, m_Loop);

    }
}
