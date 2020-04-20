using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    [SerializeField]
    Shader[] m_ShaderList;
    [SerializeField]
    bool m_MouseAutoLock = true;

    public Material m_SpectrumMaterial;
    public Material m_SpectrumMaterialSkyBlue;

    public bool m_CameraAxisXInvers = false;

    [SerializeField]
    List<AudioClip> m_BGMList;

    private void Awake()
    {
        GameManager.Instacne.m_Main = this;
        if (m_MouseAutoLock)
            MouseLock(true);

        if (m_BGMList != null && m_BGMList.Count > 0)
        {
            SoundManager.Instance.SetBGMVolume(0.2f);
            SoundManager.Instance.PlayBGM(m_BGMList);
        }
    }

    public bool IsPlayStop = false;
    public bool IsGameStop;

    public PlayerCharacter m_PlayerCharacter;
    public MainCanvas m_MainCanvas;
    public void MouseLock(bool _Lock)
    {
        if (_Lock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
