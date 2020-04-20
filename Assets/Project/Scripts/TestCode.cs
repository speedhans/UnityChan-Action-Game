using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestCode : MonoBehaviour
{
    public int m_Count;
    public float m_Pow;
    public GameObject m_Target;
    private void Update()
    {
        if (!m_Target)
        {
            CustomColorGradingPass.m_UsePass = false;
            return;
        }

        float width = Screen.width;
        float height = Screen.height;

        Vector3 campos = Camera.main.WorldToScreenPoint(m_Target.transform.position);
        Debug.Log(campos);
        Debug.Log(campos.x / width + " / " + campos.y / height); // 0 ~ 1사이

        Vector2 pos = new Vector2(campos.x / width, campos.y / height);
        if (pos.x < 0.0f || pos.x > 1.0f || pos.y < 0.0f || pos.y > 1.0f)
        {
            CustomColorGradingPass.m_BlurSampleCount = 0;
            return;
        }
        CustomColorGradingPass.m_UsePass = true;
        CustomColorGradingPass.m_BlurPoint = pos;
        CustomColorGradingPass.m_BlurPower = m_Pow;
        CustomColorGradingPass.m_BlurSampleCount = m_Count;
    }
}
