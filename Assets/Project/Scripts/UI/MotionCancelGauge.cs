using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionCancelGauge : MonoBehaviour
{
    enum TYPE
    {
        EMPTY,
        HALF,
        FULL,
    }

    [SerializeField]
    Sprite[] m_GaugeSprite = new Sprite[3];

    public UnityEngine.UI.Image[] m_GaugeImage;
    int m_GaugeMaxCount;
    int m_CurrentGauge = 0;

    private void Awake()
    {
        UIManager.Instacne.m_MotionCancelGauge = this;
        m_GaugeMaxCount = m_GaugeImage.Length * 2;

        RefreshGauge();
    }

    public int GetGauge() { return m_CurrentGauge; }

    public void AddGauge(int _Value)
    {
        m_CurrentGauge = Mathf.Clamp(m_CurrentGauge + _Value, 0, m_GaugeMaxCount);
        RefreshGauge();
    }

    public void SubGauge(int _Value)
    {
        m_CurrentGauge = Mathf.Clamp(m_CurrentGauge - _Value, 0, m_GaugeMaxCount);
        RefreshGauge();
    }

    public void SetGauge(int _Value)
    {
        m_CurrentGauge = Mathf.Clamp(_Value, 0, m_GaugeMaxCount);
        RefreshGauge();
    }

    void RefreshGauge()
    {
        int v = m_CurrentGauge;
        for (int i = 0; i < m_GaugeImage.Length; ++i)
        {
            if (v > 1)
            {
                SetGaugeState(i, TYPE.FULL);
                v -= 2;
            }
            else
            {
                SetGaugeState(i, (TYPE)v);
                v = 0;
            }
        }
    }

    void SetGaugeState(int _CellNumber, TYPE _Type)
    {
        if (_CellNumber > (m_GaugeMaxCount - 1)) return;
        m_GaugeImage[_CellNumber].sprite = m_GaugeSprite[(int)_Type];
    }
}
