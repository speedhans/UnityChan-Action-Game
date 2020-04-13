using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    static public GameManager single;
    static public GameManager Instacne
    {
        get
        {
            if (!single)
            {
                GameObject g = new GameObject("GameManager");
                single = g.AddComponent<GameManager>();
                single.Initialize();
            }
            return single;
        }
    }

    void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    public Main m_Main;
}
