using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCode : MonoBehaviour
{
    public float X = 0.0f;
    public float Y = 1.0f;
    private void Update()
    {
        Vector2 axis = new Vector2(X, Y);
        axis.Normalize();
        Debug.Log(axis + " : " + Mathf.Atan2(axis.y, axis.x) * Mathf.Rad2Deg);
    }
}
