using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraControlComponent : CharacterBaseComponent
{
    PlayerCharacter m_PlayerCharacter;

    public float m_CameraRotateYSpeed = 10.0f;
    public float m_CameraRotateXSpeed = 10.0f;
    public float m_MaximumX = 35.0f;
    public float m_WheelSpeed = 0.2f;
    public float m_CameraZoomPoint;
    public float m_CameraZoomMin = 1.5f;
    public float m_CameraZoomMax = 5.0f;
    float m_CamZOffset = 0.0f;

    public bool m_AxisXInvers = false;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_PlayerCharacter = _CharacterBase as PlayerCharacter;
        m_CameraZoomPoint = m_PlayerCharacter.m_FollowCamera.transform.localPosition.z;
        InputManager.Instacne.AddMouseWheelEvent(MouseWheelEvent);
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);
        if (GameManager.Instacne.m_Main.IsPlayStop) return;
        if (m_CharacterBase.m_Live == CharacterBase.E_Live.DEAD) return;

        Vector2 mouseAxis = InputManager.Instacne.m_Mouse2D;

        Vector3 rot = m_CharacterBase.transform.eulerAngles;
        Vector3 camrot = m_PlayerCharacter.m_FollowCameraAxis.localEulerAngles;
        if (camrot.x > 180.0f)
        {
            camrot.x -= 360.0f;
        }

        rot.y += mouseAxis.x * _DeltaTime * m_CameraRotateYSpeed;
        float camAddedX = (mouseAxis.y * _DeltaTime * m_CameraRotateXSpeed);
        camAddedX = m_AxisXInvers ? -camAddedX : camAddedX;
        camrot.x += camAddedX;
        camrot.x = Mathf.Clamp(camrot.x, -m_MaximumX, m_MaximumX);

        if (camrot.x < 0.0f)
        {
            float offset = camrot.x / -m_MaximumX;
            offset = Mathf.Lerp(m_CameraZoomMin, m_CameraZoomMax, offset);
            m_CamZOffset = offset;
            Vector3 pos = Vector3.zero;
            pos.z = ZoomClamp(m_CameraZoomPoint + m_CamZOffset);
            m_PlayerCharacter.m_FollowCamera.transform.localPosition = pos;
        }
        else if (m_CamZOffset != 0.0f)
        {
            m_CamZOffset = 0.0f;
            Vector3 pos = Vector3.zero;
            pos.z = m_CameraZoomPoint;
            m_PlayerCharacter.m_FollowCamera.transform.localPosition = pos;
        }

        m_CharacterBase.transform.eulerAngles = rot;
        m_PlayerCharacter.m_FollowCameraAxis.localEulerAngles = camrot;
    }

    public override void DestoryComponent()
    {
        base.DestoryComponent();
        InputManager.Instacne.ReleaseMouseWheelEvent(MouseWheelEvent);
    }

    void MouseWheelEvent(float _Value)
    {
        Vector3 pos = Vector3.zero;
        m_CameraZoomPoint -= _Value * Time.deltaTime * m_WheelSpeed;
        m_CameraZoomPoint = ZoomClamp(m_CameraZoomPoint);

        pos.z = m_CamZOffset != 0.0f ? ZoomClamp(m_CameraZoomPoint + m_CamZOffset) : m_CameraZoomPoint;

        m_PlayerCharacter.m_FollowCamera.transform.localPosition = pos;
    }

    float ZoomClamp(float _Value)
    {
        float min = -m_CameraZoomMin;
        float max = -m_CameraZoomMax;
        if (_Value > min)
            _Value = min;
        else if (_Value < max)
            _Value = max;

        return _Value;
    }
}
