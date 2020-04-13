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
    }
}
