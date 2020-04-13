using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeTimerWithObjectPool : MonoBehaviour
{
    public float m_LifeTime;
    float m_LifeTimer;

    Transform m_TargetTransform;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        m_LifeTimer = m_LifeTime;
    }

    public void SetTargetTransform(Transform _Transform)
    {
        m_TargetTransform = _Transform;
    }

    private void Update()
    {
        m_LifeTimer -= Time.deltaTime;// * GameManager.Instance.TimeScale;
        if (m_TargetTransform)
        {
            transform.position = m_TargetTransform.position;
        }
        if (m_LifeTimer <= 0.0f)
        {
            m_TargetTransform = null;
            ObjectPool.PushObject(gameObject);
        }
    }
}
