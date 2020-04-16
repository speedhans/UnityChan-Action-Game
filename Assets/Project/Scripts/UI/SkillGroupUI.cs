using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillGroupUI : MonoBehaviour
{
    [SerializeField]
    SkillUI[] m_SkillUI;

    private void Awake()
    {
        UIManager.Instacne.m_SkillGroupUI = this;
    }

    public void SetSkill(PlayerSkillBaseComponent _Component, int _Number)
    {
        if (_Number >= m_SkillUI.Length) return;
        m_SkillUI[_Number].SetSkill(_Component);
    }
}
