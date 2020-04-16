using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UnityEngine.UI.Image))]
public class PlayerSPBar : MonoBehaviour
{
    static readonly int m_ShaderProgressID = Shader.PropertyToID("_Progress");

    Material m_Material;

    PlayerCharacter m_Character;

    private void Awake()
    {
        UIManager.Instacne.m_PlayerSPBar = this;

        UnityEngine.UI.Image image = GetComponent<UnityEngine.UI.Image>();
        m_Material = image.material;
    }

    public void SetCharacter(PlayerCharacter _Character)
    {
        m_Character = _Character;
    }

    private void Update()
    {
        if (!m_Character)
        {
            return;
        }

        m_Material.SetFloat(m_ShaderProgressID, m_Character.m_Stemina / m_Character.m_SteminaMax);
    }
}
