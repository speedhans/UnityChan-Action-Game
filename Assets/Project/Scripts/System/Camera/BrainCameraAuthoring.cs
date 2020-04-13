using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
[RequireComponent(typeof(Cinemachine.CinemachineBrain))]
public class BrainCameraAuthoring : MonoBehaviour
{
    public Cinemachine.CinemachineBrain m_Brain { get; private set; }

    private void Start()
    {
        m_Brain = GetComponent<Cinemachine.CinemachineBrain>();
        CameraManager.Instacne.m_BrainCamera = this;
    }
}
