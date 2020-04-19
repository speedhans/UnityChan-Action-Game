using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Material m_SpectrumMaterial;
    public Material m_SpectrumMaterialSkyBlue;

    public bool m_CameraAxisXInvers = false;

    private void Awake()
    {
        GameManager.Instacne.m_Main = this;
    }

    public bool IsPlayStop = false;
    public bool IsGameStop;

    public PlayerCharacter m_PlayerCharacter;
}
