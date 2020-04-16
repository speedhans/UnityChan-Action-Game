using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : MonoBehaviour
{
    UnityEngine.UI.Image m_BackImage;
    UnityEngine.UI.Image m_FillImage;

    PlayerSkillBaseComponent m_SkillComponent;
    private void Awake()
    {
        m_BackImage = transform.Find("Background").GetComponent<UnityEngine.UI.Image>();
        m_FillImage = transform.Find("Fill").GetComponent<UnityEngine.UI.Image>();
    }

    private void Update()
    {
        if (m_SkillComponent == null) return;

        m_FillImage.fillAmount = m_SkillComponent.m_Cooldown / m_SkillComponent.m_CooldownMax;
    }

    public void SetSkill(PlayerSkillBaseComponent _Component)
    {
        m_SkillComponent = _Component;
        m_BackImage.sprite = m_SkillComponent.m_Sprite;
        m_FillImage.sprite = m_SkillComponent.m_Sprite;
    }
}
