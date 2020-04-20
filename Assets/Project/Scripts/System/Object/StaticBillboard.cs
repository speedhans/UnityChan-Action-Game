using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBillboard : MonoBehaviour
{
    Camera m_MainCamera;

    [SerializeField]
    bool FreezeVertical = false;
    [SerializeField]
    bool FreezeHorizontal = false;

    // Start is called before the first frame update
    void Start()
    {
        m_MainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_MainCamera) return;

        Vector3 myfwd = transform.forward;

        Vector3 camfwd = m_MainCamera.transform.forward;
        if (FreezeHorizontal)
        {
            camfwd.x = myfwd.x;
            camfwd.z = myfwd.z;
        }
        if (FreezeVertical)
        {
            camfwd.y = myfwd.y;
        }
        transform.forward = camfwd;
    }
}
