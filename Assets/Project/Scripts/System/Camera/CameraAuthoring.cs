using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Cinemachine.CinemachineVirtualCameraBase))]
public class CameraAuthoring : MonoBehaviour
{
    [SerializeField]
    int m_CameraNumber;
    public Cinemachine.CinemachineVirtualCameraBase m_VirtualCamera { get; private set; }

    [Tooltip("이 옵션은 게임 시작시 카메라의 우선순위를 해당 값으로 자동으로 맞춰줍니다. 시작시의 메인 카메라를 지정하기 위해서는 반드시 해당 카메라만 우선순위를 높게 잡아주세요")]
    [SerializeField]
    int m_CameraPriority = 0;

    [Tooltip("카메라의 우선순위가 높아질때 카메라간 위치가 블랜딩되는 시간")]
    [SerializeField]
    float m_CameraBlendTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_VirtualCamera = GetComponent<Cinemachine.CinemachineVirtualCameraBase>();
        m_VirtualCamera.Priority = m_CameraPriority;
        CameraManager.Instacne.InsertCamera(m_CameraNumber, this);
    }

    public void SetBlendTime(float _BlendTime)
    {
        m_CameraBlendTime = _BlendTime;
    }

    public void SetFocus()
    {
        CameraManager.Instacne.ChangeFocus(this, m_CameraBlendTime);
    }

    public void ResetPriority()
    {
        m_VirtualCamera.Priority = m_CameraPriority;
    }
}
