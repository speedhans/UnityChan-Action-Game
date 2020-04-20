using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectModeUIManager : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_Text m_TipText;

    int m_Level = -1;

    public void TutorialButton()
    {
        m_Level = 0;
        m_TipText.text = "게임의 조작방법을 간략하게 알려드리는 모드입니다 (진짜 간략하게)";
    }

    public void NormalModeButton()
    {
        m_Level = 1;
        m_TipText.text = "일반 모드입니다. 플레이어의 체력이 초당 2씩 회복됩니다. 처음하신다면 이 모드를 추천드립니다";
    }

    public void HardModeButton()
    {
        m_Level = 2;
        m_TipText.text = "어려움 모드입니다. 플레이어의 체력이 초당 0.5씩 회복되며, 적이 특수기를 게임 시작부터 사용합니다";
    }

    public void StartButton()
    {
        if (m_Level == 0)
        {
            GameManager.Instacne.m_GameLevel = 1;
            SceneManager.Instance.LoadScene("Tutorial");
        }
        else
        {
            GameManager.Instacne.m_GameLevel = m_Level;
            SceneManager.Instance.LoadScene("Stage_01");
        }
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
