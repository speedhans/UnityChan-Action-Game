using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PopupMenu : MonoBehaviour
{
    [SerializeField]
    GameObject m_MenuList;

    bool Destroy = false;
    private void Awake()
    {
        InputManager.Instacne.AddKeyEvent("escape", EscapeButton);
    }

    private void OnDestroy()
    {
        if (Destroy) return;
        InputManager.Instacne.ReleaseKeyEvent("escape", EscapeButton);
    }

    void OnApplicationQuit()
    {
        Destroy = true;
    }

    void EscapeButton(InputActionPhase _Phase)
    {
        if (_Phase != InputActionPhase.Started) return;

        if (m_MenuList.activeSelf)
        {
            Time.timeScale = 1.0f;
            m_MenuList.SetActive(false);

            GameManager.Instacne.m_Main.IsGameStop = false;
            GameManager.Instacne.m_Main.MouseLock(true);
        }
        else
        {
            Time.timeScale = 0.0f;
            m_MenuList.SetActive(true);

            GameManager.Instacne.m_Main.IsGameStop = true;
            GameManager.Instacne.m_Main.MouseLock(false);
        }
    }

    public void ReturnMain()
    {
        SceneManager.Instance.LoadScene("Select");
    }

    public void ExitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
