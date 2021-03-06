﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    static public UIManager single;
    static public UIManager Instacne
    {
        get
        {
            if (!single)
            {
                GameObject g = new GameObject("UIManager");
                single = g.AddComponent<UIManager>();
            }
            return single;
        }
    }

    public PlayerHPBar m_PlayerHPBar;
    public PlayerSPBar m_PlayerSPBar;
    public MotionCancelGauge m_MotionCancelGauge;
    public ComboViewer m_ComboViewer;
    public SkillGroupUI m_SkillGroupUI;
    public EnemyHPBar m_EnemyHPBar;
}
