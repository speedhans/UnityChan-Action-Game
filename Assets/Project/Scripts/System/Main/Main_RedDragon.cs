using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_RedDragon : Main
{
    [SerializeField]
    FireDragonCharacter m_Dragon;
    [SerializeField]
    GameObject m_LandingSmokePrefab;
    [SerializeField]
    CameraAuthoring m_StartCimemaCam;

    private void Start()
    {
        GameManager.Instacne.m_Main.IsPlayStop = true;

        m_StartCimemaCam.SetFocus();
        m_Dragon.transform.position = m_Dragon.transform.position + Vector3.up * 10.0f;
        m_Dragon.m_Animator.CrossFade("FlyStand",0);
        StartCoroutine(C_DragonLanding());
    }

    IEnumerator C_DragonLanding()
    {
        m_Dragon.m_Rigidbody.velocity = Vector3.up * 1.5f;
        bool land = false;
        while(!land)
        {
            if (Physics.Raycast(m_Dragon.transform.position, Vector3.down, 0.5f, 1 << LayerMask.NameToLayer("Ground")))
            {
                m_Dragon.m_Rigidbody.velocity = Vector3.up * 0.5f;
                m_Dragon.m_Animator.CrossFade("FlyLand", 0.2f);
                land = true;
            }
            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_LandingSmokePrefab.name);
        life.Initialize();
        life.transform.position = m_Dragon.transform.position;
        life.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.0f);

        m_Dragon.m_Animator.CrossFade("Roer1", 0.15f);

        yield return new WaitForSeconds(5.0f);

        m_StartCimemaCam.ResetPriority();

        yield return new WaitForSeconds(1.5f);

        GameManager.Instacne.m_Main.IsPlayStop = false;
    }
}
