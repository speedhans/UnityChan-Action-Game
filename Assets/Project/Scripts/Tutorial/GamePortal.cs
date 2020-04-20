using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePortal : MonoBehaviour
{
    public enum E_TYPE
    {
        NORMAL = 1,
        HARD = 2,
    }

    [SerializeField]
    E_TYPE m_Type;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        if (other.GetComponentInParent<PlayerCharacter>() == null) return;

        GameManager.Instacne.m_GameLevel = (int)m_Type;
        SceneManager.Instance.LoadScene("Stage_01");
    }
}
