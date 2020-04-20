﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    static GameObject _gameobject = null;
    static SceneManager single = null;
    public static SceneManager Instance
    {
        get
        {
            if (!single)
            {
                _gameobject = new GameObject("SceneManager");
                single = _gameobject.AddComponent<SceneManager>();
                single.Initialize();
            }

            return single;
        }

        private set { }
    }

    void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    Coroutine m_LoadingCoroutine;
    public LoadingManager m_LoadingManager;

    public void LoadScene(string _SceneName)
    {
        Time.timeScale = 1.0f;
        if (m_LoadingCoroutine != null) StopCoroutine(m_LoadingCoroutine);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");
        m_LoadingCoroutine = StartCoroutine(C_Loading(_SceneName));
    }

    public void LoadSceneDirect(string _SceneName)
    {
        Time.timeScale = 1.0f;
        if (m_LoadingCoroutine != null) StopCoroutine(m_LoadingCoroutine);
        UnityEngine.SceneManagement.SceneManager.LoadScene(_SceneName);
    }

    IEnumerator C_Loading(string _NextSceneName)
    {
        yield return new WaitForSeconds(1.0f); // 임시

        while (m_LoadingManager == null) yield return null;


        AsyncOperation Op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(_NextSceneName);
        Op.allowSceneActivation = false;

        float progress = 0.4f;
        while(!Op.isDone)
        {
            float fullprogress = ((Op.progress / 0.9f) * 0.6f) + progress;
            m_LoadingManager.SetValue(fullprogress);
            if (fullprogress >= 1.0f)
            {
                m_LoadingManager.SetValue(1.0f);
                yield return new WaitForSeconds(1.0f);
                Op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
