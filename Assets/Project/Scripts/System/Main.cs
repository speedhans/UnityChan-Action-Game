using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public Material m_SpectrumMaterial;
    public Material m_SpectrumMaterialSkyBlue;

    public bool m_CameraAxisXInvers = false;

    public AudioClip m_FootClip1;

    [SerializeField]
    bool m_PlayStop;
    [SerializeField]
    bool m_GameStop;

    private void Awake()
    {
        GameManager.Instacne.m_Main = this;
    }

    public bool IsPlayStop() { return m_PlayStop; }
    public bool IsGameStop() { return m_GameStop; }
}
