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

    [SerializeField]
    GameTimer m_GameTimer;

    private void Start()
    {
        GameManager.Instacne.m_Main.IsPlayStop = true;

        m_StartCimemaCam.SetFocus();
        m_Dragon.m_DeadEvent += BossDead;
        m_Dragon.transform.position = m_Dragon.transform.position + Vector3.up * 10.0f;
        m_Dragon.m_Animator.CrossFade("FlyStand", 0);
        StartCoroutine(C_DragonLanding());
    }

    IEnumerator C_DragonLanding()
    {
        m_MainCanvas.gameObject.SetActive(false);

        m_Dragon.m_Rigidbody.velocity = Vector3.up * 1.5f;
        bool land = false;
        while (!land)
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
        yield return new WaitForSeconds(1.5f);
        SetPointBlur(m_Dragon.m_HeadPoint.position, 0, 0);
        CustomColorGradingPass.m_UsePass = true;
        float roartime = 0.0f;
        while (roartime < 1.5f)
        {
            roartime += Time.deltaTime;
            SetPointBlur(m_Dragon.m_HeadPoint.position, roartime * 0.01f, roartime * 10);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        roartime = 1.0f;
        while (roartime > 0.0f)
        {
            roartime -= Time.deltaTime;
            SetPointBlur(m_Dragon.m_HeadPoint.position, roartime * 0.015f, roartime * 10);
            yield return null;
        }
        CustomColorGradingPass.m_UsePass = false;
        m_StartCimemaCam.ResetPriority();

        yield return new WaitForSeconds(1.5f);

        GameManager.Instacne.m_Main.IsPlayStop = false;

        m_MainCanvas.gameObject.SetActive(true);
        m_GameTimer.Visible(true);
        m_GameTimer.m_IsStop = false;
    }

    void SetPointBlur(Vector3 _Point, float _Power, float _SampleCount)
    {
        Vector2 screensize = new Vector2(Screen.width, Screen.height);
        Vector3 campos = Camera.main.WorldToScreenPoint(_Point);

        Vector2 pos = new Vector2(campos.x / screensize.x, campos.y / screensize.y);
        if (pos.x < 0.0f || pos.x > 1.0f || pos.y < 0.0f || pos.y > 1.0f)
        {
            CustomColorGradingPass.m_BlurSampleCount = 0;
            return;
        }

        CustomColorGradingPass.SetPassData(pos, Mathf.Clamp(_Power, 0.0f, 1.0f), _SampleCount);
    }

    void BossDead()
    {
        m_GameTimer.m_IsStop = true;
    }
}
