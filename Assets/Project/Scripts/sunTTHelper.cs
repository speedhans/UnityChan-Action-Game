using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sunTT
{
    public class sunTTHelper
    {
        static public void SetLocalTransformIdentity(Transform _Transform)
        {
            _Transform.localPosition = Vector3.zero;
            _Transform.localRotation = Quaternion.identity;
            _Transform.localScale = Vector3.one;
        }

        static public void SetLocalTransform(Transform _Transform, Vector3 _Position, Quaternion _Rotation, Vector3 _Scale)
        {
            _Transform.localPosition = _Position;
            _Transform.localRotation = _Rotation;
            _Transform.localScale = _Scale;
        }

        static public void SetLocalTransform(Transform _Transform, Vector3 _Position, Quaternion _Rotation)
        {
            _Transform.localPosition = _Position;
            _Transform.localRotation = _Rotation;
        }

        static public Vector3 GetVectorFromAngle(float _Angle)
        {
            float angleRad = _Angle * (Mathf.PI / 180.0f);
            return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        }

        static public float GetAngleFromVector(Vector3 _Direction)
        {
            Vector3 dir = _Direction.normalized;
            float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (n < 0.0f) n += 360.0f;
            return n;
        }

        static public float GetTriangleVericalLength(float _Angle, float _StraightLength) //  인수 <A 각, 밑변의 길이 로 직변의 길이구하기
        {
            return _StraightLength * Mathf.Tan(_Angle * (Mathf.PI / 180));
        }

        static public float GetTriangleDiagonalLength(float _Angle, float _StraightLength) // 인수 <A 각, 직변으로 대각의 길이 구하기
        {
            return _StraightLength / Mathf.Cos(_Angle * (Mathf.PI / 180));
        }

        static public Vector3 AbsVector3(Vector3 _Source)
        {
            Vector3 v = Vector3.zero;
            if (_Source.x < 0.0f)
                v.x = _Source.x * -1.0f;
            if (_Source.y < 0.0f)
                v.y = _Source.y * -1.0f;
            if (_Source.z < 0.0f)
                v.z = _Source.z * -1.0f;
            return v;
        }

        /*
         * w 는 depth
         */
        static public Vector3 ScreenPointToWorldManual(Camera _Camera, Vector3 _Position)
        {
            Matrix4x4 world2Screen = _Camera.projectionMatrix * _Camera.worldToCameraMatrix;
            Matrix4x4 screen2World = world2Screen.inverse;

            float[] inn = new float[4];

            inn[0] = 2.0f * (_Position.x / _Camera.pixelWidth) - 1.0f;
            inn[1] = 2.0f * (_Position.y / _Camera.pixelHeight) - 1.0f;
            inn[2] = _Camera.nearClipPlane;
            inn[3] = 1.0f;

            Vector4 pos = screen2World * new Vector4(inn[0], inn[1], inn[2], inn[3]);

            pos.w = 1.0f / pos.w;

            pos.x *= pos.w;
            pos.y *= pos.w;
            pos.z *= pos.w;

            return new Vector3(pos.x, pos.y, pos.z);
        }
    }
}
