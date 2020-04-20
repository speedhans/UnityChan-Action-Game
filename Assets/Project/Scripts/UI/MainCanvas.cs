using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCanvas : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameManager.Instacne.m_Main.m_MainCanvas = this;
    }
}
