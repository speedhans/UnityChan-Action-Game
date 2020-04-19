using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboViewer : MonoBehaviour
{
    [SerializeField]
    GameObject m_TextList;
    [SerializeField]
    TMPro.TMP_Text m_CountText;

    int m_FontMinSize = 60;
    int m_FontMaxSize = 120;
    float m_IncreaseSpeed = 350.0f;
    float m_DecreaseSpeed = 500.0f;

    Coroutine m_TextAnimationCoroutine;

    int m_CurrentCombo = 0;

    [SerializeField]
    float m_ComboLinkDuration;
    float m_ComboLinkDurationTimer;

    private void Awake()
    {
        UIManager.Instacne.m_ComboViewer = this;
        m_CountText.fontSize = m_FontMinSize;

        m_CurrentCombo = 0;
        m_TextList.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (m_ComboLinkDurationTimer > 0.0f)
        {
            m_ComboLinkDurationTimer -= Time.deltaTime;
            if (m_ComboLinkDurationTimer <= 0.0f)
            {
                m_CurrentCombo = 0;
                m_TextList.gameObject.SetActive(false);
                if (m_TextAnimationCoroutine != null)
                {
                    StopCoroutine(m_TextAnimationCoroutine);
                    m_TextAnimationCoroutine = null;
                }
            }
        }
    }

    public void AddCombo()
    {
        ++m_CurrentCombo;
        if (m_CurrentCombo < 2) return;

        m_CountText.text = m_CurrentCombo.ToString();

        if (m_TextAnimationCoroutine != null)
        {
            StopCoroutine(m_TextAnimationCoroutine);
            m_CountText.fontSize = m_FontMinSize;
        }
        m_TextList.gameObject.SetActive(true);
        m_ComboLinkDurationTimer = m_ComboLinkDuration;
        m_TextAnimationCoroutine = StartCoroutine(C_CountTextAnimation());
    }

    IEnumerator C_CountTextAnimation()
    {
        while(m_CountText.fontSize < m_FontMaxSize)
        {
            m_CountText.fontSize = m_CountText.fontSize + Time.deltaTime * m_IncreaseSpeed;
            yield return null;
        }
        m_CountText.fontSize = m_FontMaxSize;
        while (m_CountText.fontSize > m_FontMinSize)
        {
            m_CountText.fontSize = m_CountText.fontSize - Time.deltaTime * m_DecreaseSpeed;
            yield return null;
        }
        m_CountText.fontSize = m_FontMinSize;
        m_TextAnimationCoroutine = null;
    }

    public int GetCurrentCombo() { return m_CurrentCombo; }
}
