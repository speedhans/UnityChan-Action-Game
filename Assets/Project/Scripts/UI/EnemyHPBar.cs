using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class EnemyHPBar : MonoBehaviour
{
    static readonly int m_ShaderProgressID = Shader.PropertyToID("_Progress");

    UnityEngine.UI.Image m_Image;
    Material m_Material;

    CharacterBase m_Character;
    [SerializeField]
    TMPro.TMP_Text m_NameText;
    private void Awake()
    {
        UIManager.Instacne.m_EnemyHPBar = this;

        m_NameText = GetComponentInChildren<TMPro.TMP_Text>();
        m_Image = GetComponent<UnityEngine.UI.Image>();
        m_Material = m_Image.material;
        m_Image.enabled = false;
        m_NameText.enabled = false;
    }

    public void SetCharacter(CharacterBase _Character)
    {
        m_Character = _Character;
        m_NameText.text = m_Character.gameObject.name;
        m_Image.enabled = true;
        m_NameText.enabled = true;
    }

    private void Update()
    {
        if (!m_Character)
        {
            return;
        }

        m_Material.SetFloat(m_ShaderProgressID, m_Character.m_Health / m_Character.m_HealthMax);
    }
}
