using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonMiniMeteorComponent : FireDragonBaseComponent
{
    readonly int m_AnimKeyFlyStart = Animator.StringToHash("FlyStart");
    readonly int m_AnimKeyFlyLand = Animator.StringToHash("FlyLand");

    readonly string m_MeteorPrefab = "MiniMeteor";
    readonly string m_FlySmokePrefab = "DragonFlySmoke";
    readonly string m_SkySmokePrefb = "DragonMeteorSkySmoke";

    public float m_Cooldown = 20.0f;
    public float m_CooldownTimer;

    public float m_FlyTime = 1.5f;
    public float m_FlyTimer;

    public float m_Duration = 6.0f;
    public float m_DurationTimer;

    public float m_LandingTime = 1.65f;
    public float m_LandingTimer;

    public float m_MeteorSpawnDelay = 0.25f;
    public float m_MeteorSpawnTimer;

    float m_MeteorDamage = 20.0f;

    bool m_UseMeteor = false;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_Frequency = 20;
        m_CharacterBase.m_AnimCallback.AddMotionEndEvent(Land, 31);
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        MotionProgress(_DeltaTime);
    }

    public override void FixedUpdateComponent(float _FixedDeltaTime)
    {
        base.FixedUpdateComponent(_FixedDeltaTime);
        if (m_CooldownTimer > 0.0f)
        {
            m_CooldownTimer -= _FixedDeltaTime;
            return;
        }
        if (!DefaultStateCheck()) return;
        if (!DragonStateCheck()) return;
        if (!m_AICharacter.m_TargetCharacter) return;
        if (!ConditionCheck()) return;
        if (!CalculateFrequency((int)(m_AICharacter.GetTargetData().Distance))) return;

        StartNewMotion();
        m_CooldownTimer = m_Cooldown;
        UseMeteor();
    }

    void MotionProgress(float _DeltaTime)
    {
        if (!m_UseMeteor) return;

        if (m_FlyTimer > 0.0f)
        {
            m_FlyTimer -= _DeltaTime;
            m_CharacterBase.m_Rigidbody.velocity = Vector3.up * 5.5f;

            if (m_FlyTimer <= 0.5f)
                m_CharacterBase.m_Animator.SetLayerWeight(1, (1.0f - m_FlyTimer * 2.0f));

            if (m_FlyTimer <= 0.0f)
            {
                m_DurationTimer = m_Duration;
                SoundManager.Instance.PlayDefaultSound(m_FireDragonCharacter.m_RoarClip, 0.8f);

                Vector3 effpos = m_AICharacter.transform.position;
                effpos.y = 20.0f;
                LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_SkySmokePrefb);
                life.Initialize();
                life.transform.position = effpos;
                life.gameObject.SetActive(true);
            }
            return;
        }

        if (m_DurationTimer > 0.0f)
        {
            m_DurationTimer -= _DeltaTime;
            m_CharacterBase.m_Rigidbody.velocity = Vector3.up * 1.0f;

            if (m_DurationTimer <= 0.5f)
                m_CharacterBase.m_Animator.SetLayerWeight(1, m_DurationTimer * 2.0f);
            else
            {
                if (m_MeteorSpawnTimer > 0.0f)
                {
                    m_MeteorSpawnTimer -= _DeltaTime;
                    if (m_MeteorSpawnTimer <= 0.0f)
                    {
                        m_MeteorSpawnTimer = m_MeteorSpawnDelay;

                        Vector3 mpos = m_AICharacter.transform.position;
                        mpos.y = 25.0f;
                        Vector3 direction = Vector3.zero;
                        if (Random.Range(0, 100) > 40 || m_AICharacter.m_TargetCharacter == null)
                        {
                            mpos.x += Random.Range(-25, 25);
                            mpos.z += Random.Range(-25, 25);
                            direction = new Vector3(Random.Range(-0.5f, 0.5f), -1.0f, Random.Range(-0.5f, 0.5f));
                        }
                        else
                        {
                            Vector3 targetpos = m_AICharacter.m_TargetCharacter.transform.position;
                            targetpos.y = 0.0f;
                            targetpos.x += Random.Range(-5, 5);
                            targetpos.z += Random.Range(-5, 5);
                            mpos.x = targetpos.x;
                            mpos.z = targetpos.z;
                            direction = new Vector3(Random.Range(-0.25f, 0.25f), -1.0f, Random.Range(-0.25f, 0.25f));
                        }
                        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_MeteorPrefab);
                        life.Initialize();
                        life.transform.position = mpos;

                        MiniMeteorComponent meteor = life.GetComponent<MiniMeteorComponent>();
                        meteor.Initialize(m_FireDragonCharacter, direction, 10.0f, m_MeteorDamage, 3.5f);

                        life.gameObject.SetActive(true);
                    }
                }
            }

            if (m_DurationTimer <= 0.0f)
            {
                m_LandingTimer = m_LandingTime;
            }
            return;
        }

        if (m_LandingTimer > 0.0f)
        {
            m_LandingTimer -= _DeltaTime;
            if (m_LandingTimer <= 0.0f)
            {
                m_CharacterBase.m_Animator.CrossFade(m_AnimKeyFlyLand, 0.15f);

                LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_FlySmokePrefab);
                life.Initialize();
                life.transform.position = m_FireDragonCharacter.transform.position + Vector3.down;
                life.gameObject.SetActive(true);
            }
        }
    }

    public void UseMeteor()
    {
        m_UseMeteor = true;
        m_CharacterBase.m_Immortal = true;
        m_CharacterBase.m_Animator.CrossFade(m_AnimKeyFlyStart, 0.3f);
        m_FlyTimer = m_FlyTime;
        m_MeteorSpawnTimer = m_MeteorSpawnDelay;
        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_FlySmokePrefab);
        life.Initialize();
        life.transform.position = m_FireDragonCharacter.transform.position;
        life.gameObject.SetActive(true);
    }

    void Land()
    {
        m_UseMeteor = false;
        m_CharacterBase.m_Immortal = false;
    }
}
