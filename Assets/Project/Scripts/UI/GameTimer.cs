using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTimer : MonoBehaviour
{
    public bool m_IsStop = true;

    [SerializeField]
    TMPro.TMP_Text m_TextMinute;
    [SerializeField]
    TMPro.TMP_Text m_TextCenter;
    [SerializeField]
    TMPro.TMP_Text m_TextSecond;

    float m_Time;

    private void Awake()
    {
        Visible(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsStop) return;

        m_Time += Time.deltaTime;
        m_TextMinute.text = ((int)m_Time / 60).ToString();
        m_TextSecond.text = ((int)m_Time % 60).ToString();
    }

    public void Visible(bool _Visible)
    {
        m_TextMinute.enabled = _Visible;
        m_TextCenter.enabled = _Visible;
        m_TextSecond.enabled = _Visible;
    }
}
