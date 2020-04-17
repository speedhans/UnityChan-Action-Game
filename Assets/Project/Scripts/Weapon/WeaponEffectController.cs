using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEffectController : MonoBehaviour
{
    static readonly int m_WeaponLighingKey = Shader.PropertyToID("_LightPow");
    float m_LightPower = 0.0f;
    public float m_LightIncreaseSpeed = 2.0f;
    public float m_StartValue = 0.0f;
    public Material m_Material;
    public GameObject m_EnableEffect;
    public GameObject m_DisableEffect;

    XftWeapon.XWeaponTrail m_Trail;
    public float m_TrailEnableTime;
    float m_TrailEnableTimer;

    bool m_First = true;
    private void Awake()
    {
        m_Material = new Material(m_Material);
        MeshRenderer renderer = GetComponentInChildren<MeshRenderer>();
        Material[] oriMats = renderer.materials;
        Material[] mats = new Material[oriMats.Length + 1];
        for (int i = 0; i < oriMats.Length; ++i)
        {
            mats[i] = oriMats[i];
        }

        mats[mats.Length - 1] = m_Material;
        renderer.materials = mats;

        m_Trail = GetComponentInChildren<XftWeapon.XWeaponTrail>();
        m_TrailEnableTimer = m_TrailEnableTime;
        if (m_TrailEnableTimer > 0.0f)
            m_Trail.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        m_LightPower = m_StartValue;

        if (m_First) return;

        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_EnableEffect.name);
        life.Initialize();
        life.SetTargetTransform(this.transform);
        life.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        if (m_First)
        {
            m_First = false;
            return;
        }
        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_DisableEffect.name);
        life.Initialize();
        life.transform.position = this.transform.position;
        life.gameObject.SetActive(true);

        m_TrailEnableTimer = m_TrailEnableTime;
        if (m_TrailEnableTimer > 0.0f)
            m_Trail.gameObject.SetActive(false);
    }

    private void Update()
    {
        float deltatime = Time.deltaTime;
        m_LightPower += deltatime * m_LightIncreaseSpeed;
        m_Material.SetFloat(m_WeaponLighingKey, m_LightPower);

        if (m_TrailEnableTimer > 0.0f)
        {
            m_TrailEnableTimer -= deltatime;
            if (m_TrailEnableTimer <= 0.0f)
            {
                m_Trail.Activate();
                m_Trail.gameObject.SetActive(true);
            }
        }
    }
}
