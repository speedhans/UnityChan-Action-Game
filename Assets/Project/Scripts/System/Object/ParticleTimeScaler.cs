using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTimeScaler : MonoBehaviour
{
    ParticleSystem[] m_Particles;

    // Start is called before the first frame update
    void Awake()
    {
        m_Particles = GetComponentsInChildren<ParticleSystem>();
    }

    private void Update()
    {
        float deltatime = Time.deltaTime;// * GameManager.Instance.TimeScale;
        foreach(ParticleSystem p in m_Particles)
        {
            p.Simulate(deltatime, false, false);
        }
    }
}
