using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    static public CameraManager single;
    static public CameraManager Instacne
    {
        get
        {
            if (!single)
            {
                GameObject g = new GameObject("CameraManager");
                single = g.AddComponent<CameraManager>();
                single.Initialize();
            }
            return single;
        }
    }

    void Initialize()
    {

    }

    public BrainCameraAuthoring m_BrainCamera;
    Dictionary<int, CameraAuthoring> m_CameraDic = new Dictionary<int, CameraAuthoring>();

    public void InsertCamera(int _Number, CameraAuthoring _Camera)
    {
        CameraAuthoring camera = null;
        if (m_CameraDic.TryGetValue(_Number, out camera))
        {
            Debug.Log("Camera with the same number cannot be registered");
            return;
        }

        m_CameraDic.Add(_Number, _Camera);
    }

    public void RemoveCamera(int _Number)
    {
        if (m_CameraDic.Remove(_Number))
        {
            Debug.Log("Remove Camera Data " + _Number.ToString());
        }
    }

    public void ChangeFocus(CameraAuthoring _Camera, float _BlendTime)
    {
        foreach(CameraAuthoring c in m_CameraDic.Values)
        {
            c.ResetPriority();
        }

        m_BrainCamera.m_Brain.m_DefaultBlend.m_Time = _BlendTime;
        _Camera.m_VirtualCamera.Priority = 99;
    }
}
