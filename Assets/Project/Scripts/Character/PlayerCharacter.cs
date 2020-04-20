using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sunTT;

public class PlayerCharacter : CharacterBase
{
    public float m_Stemina;
    public float m_SteminaMax;

    public CameraAuthoring m_FollowCamera;
    public Transform m_FollowCameraAxis { get; protected set; }
    public CameraAuthoring m_FinishCamera;

    public AudioClip m_NormalAttackClip;
    public AudioClip m_SkillAttackClip1;
    public AudioClip m_SkillAttackClip2;
    public AudioClip m_SkillAttackClip3;
    public AudioClip m_FinishAttackClip1;

    public GameObject m_SwordPrefab;
    public GameObject m_TwoSwordPrefab;
    public GameObject m_HalbertPrefab;
    public GameObject m_KatanaPrefab;

    [SerializeField]
    AudioClip m_MotionCancelAudioClip;
    Coroutine m_CoroutineRimProgress;

    protected override void Awake()
    {
        base.Awake();

        GameManager.Instacne.m_Main.m_PlayerCharacter = this;

        m_Team = E_Team.RED;

        SetHand();

        if (!m_FollowCamera)
        {
            m_FollowCamera = transform.Find("CamAxis/Cam Follow").GetComponent<CameraAuthoring>();
            m_FollowCameraAxis = m_FollowCamera.transform.parent;
        }
        if (!m_FinishCamera)
        {
            m_FinishCamera = transform.Find("CamAxis/Cam Finish").GetComponent<CameraAuthoring>();
        }

        SetComponent<PlayerMovementComponent>(this);
        PlayerCameraControlComponent CamRot = SetComponent<PlayerCameraControlComponent>(this);
        CamRot.m_AxisXInvers = GameManager.Instacne.m_Main.m_CameraAxisXInvers;
        SetComponent<PlayerDashComponent>(this);
        SetComponent<PlayerNormalAttackComponent>(this);
        SetComponent<PlayerSkillSwordAttackComponent>(this);
        SetComponent<PlayerSkillTwoHandSwordAttackComponent>(this);
        SetComponent<PlayerSkillHalberdAttackComponent>(this);
        SetComponent<PlayerSkillKatanaAttackComponent>(this);
        SetComponent<PlayerSkillFinishPunchComponent>(this);
    }

    private void Start()
    {
        UIManager.Instacne.m_PlayerHPBar.SetCharacter(this);
        UIManager.Instacne.m_PlayerSPBar.SetCharacter(this);
    }

    void SetHand()
    {
        Transform left = FindBone(m_Animator.transform, "LeftHand");
        Transform right = FindBone(m_Animator.transform, "RightHand");

        m_LeftHandPoint = new GameObject("LeftAttachPoint").transform;
        m_RightHandPoint = new GameObject("RightAttachPoint").transform;

        m_LeftHandPoint.SetParent(left);
        sunTTHelper.SetLocalTransform(m_LeftHandPoint, new Vector3(-0.075f, 0.0f, 0.02f), Quaternion.Euler(0.0f, -90.0f, 0.0f));
        m_RightHandPoint.SetParent(right);
        sunTTHelper.SetLocalTransform(m_RightHandPoint, new Vector3(-0.0695f, 0.0f, 0.0116f), Quaternion.Euler(0.0f, -90.0f, 0.0f));

        //sunTTHelper.SetLocalTransformIdentity(Instantiate(m_SwordPrefab, m_LeftHandPoint).transform);
        m_SwordPrefab = Instantiate(m_SwordPrefab, m_RightHandPoint);
        sunTTHelper.SetLocalTransformIdentity(m_SwordPrefab.transform);
        m_SwordPrefab.SetActive(false);

        m_TwoSwordPrefab = Instantiate(m_TwoSwordPrefab, m_RightHandPoint);
        sunTTHelper.SetLocalTransformIdentity(m_TwoSwordPrefab.transform);
        m_TwoSwordPrefab.SetActive(false);

        m_HalbertPrefab = Instantiate(m_HalbertPrefab, m_RightHandPoint);
        sunTTHelper.SetLocalTransformIdentity(m_HalbertPrefab.transform);
        m_HalbertPrefab.SetActive(false);

        m_KatanaPrefab = Instantiate(m_KatanaPrefab, m_RightHandPoint);
        sunTTHelper.SetLocalTransformIdentity(m_KatanaPrefab.transform);
        m_KatanaPrefab.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
        float deltatime = Time.deltaTime;
        if (m_Live != E_Live.DEAD && !m_StopCharacter)
        {
            AddHealth(GameManager.Instacne.m_GameLevel == 2 ? 0.5f * deltatime : 2.0f * deltatime);
            AddStemina(2.0f * deltatime);
        }
    }

    public void StartMotionCancelRim(float _Pow = 5.0f, float _Duration = 1.0f)
    {
        SoundManager.Instance.PlayDefaultSound(m_MotionCancelAudioClip);
        if (m_CoroutineRimProgress != null) StopCoroutine(m_CoroutineRimProgress);
        m_CoroutineRimProgress = StartCoroutine(C_RimProgress(_Pow, _Duration));
        m_IsDashing = false;
        m_Rigidbody.velocity = Vector3.zero;
    }

    IEnumerator C_RimProgress(float _StartPow = 5.0f, float _Duration = 1.0f)
    {
        float pow = _StartPow;
        float timer = _Duration;
        float decreasepow = pow / timer;

        while(timer > 0.0f)
        {
            float deltatime = Time.deltaTime;
            pow -= decreasepow * deltatime;
            SetRim(pow);
            timer -= deltatime;
            yield return null;
        }
        SetRim(0.0f);
        m_CoroutineRimProgress = null;
    }

    public void SetRim(float _Pow)
    {
        for (int i = 0; i < m_Renderers.Length; ++i)
        {
            foreach (Material m in m_Renderers[i].materials)
            {
                m.SetFloat("_RimPow", _Pow);
            }
        }
    }

    public override void MotionEnd(int _Value)
    {
        base.MotionEnd(_Value);
        WeaponActivate(_Value, false);
        m_FollowCamera.SetFocus();
    }

    public void AllWeaponDisable()
    {
        m_SwordPrefab.SetActive(false);
        m_TwoSwordPrefab.SetActive(false);
        m_HalbertPrefab.SetActive(false);
        m_KatanaPrefab.SetActive(false);
    }

    public void ActivateOnlyOneWeapon(int _Number)
    {
        AllWeaponDisable();
        WeaponActivate(_Number, true);
    }

    public void WeaponActivate(int _Number, bool _Active)
    {
        switch (_Number)
        {
            case 11:
                m_SwordPrefab.SetActive(_Active);
                break;
            case 12:
                m_TwoSwordPrefab.SetActive(_Active);
                break;
            case 13:
                m_HalbertPrefab.SetActive(_Active);
                break;
            case 14:
                m_KatanaPrefab.SetActive(_Active);
                break;
        }
    }

    public override void GiveToDamage(CharacterBase _Attacker, float _Damage, bool _Knockback = false)
    {
        if (m_Immortal) return;
        if (m_Down) return;

        if (m_IsDashing)
        {
            CreateSpectrumMesh(1.5f, true, GameManager.Instacne.m_Main.m_SpectrumMaterialSkyBlue);
            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>("AvoidEffect");
            life.Initialize();
            life.transform.position = transform.position + Vector3.up;
            life.gameObject.SetActive(true);
            UIManager.Instacne.m_MotionCancelGauge.AddGauge(2);
            return;
        }

        if (m_UseHitAnimation)
        {
            if (m_Health - _Damage > 0.0f)
            {
                if (!_Knockback)
                {
                    int number = Random.Range(0, 100);
                    SoundManager.Instance.PlayDefaultSound(number > 50 ? m_AudioListHit[0] : m_AudioListHit[1]);
                }
                else
                {
                    SoundManager.Instance.PlayDefaultSound(m_AudioListHit[2]);
                }
            }
            AllWeaponDisable();
        }

        base.GiveToDamage(_Attacker, _Damage, _Knockback);
    }

    public void AddStemina(float _Value)
    {
        m_Stemina = Mathf.Clamp(m_Stemina + _Value, 0.0f, m_SteminaMax);
    }

    public void SubStemina(float _Value)
    {
        m_Stemina = Mathf.Clamp(m_Stemina - _Value, 0.0f, m_SteminaMax);
    }

    public void AddHealth(float _Value)
    {
        m_Health = Mathf.Clamp(m_Health + _Value, 0.0f, m_HealthMax);
    }

    public void SubHealth(float _Value)
    {
        m_Health = Mathf.Clamp(m_Health - _Value, 0.0f, m_HealthMax);
    }

    public void CreateSpectrumMesh(float _Duration, bool _AlphaDecrease, Material _Material)
    {
        GameObject g = Instantiate(m_Animator.gameObject);
        foreach(WeaponEffectController w in g.GetComponentsInChildren<WeaponEffectController>())
        {
            w.enabled = false;
        }
        g.transform.SetPositionAndRotation(transform.position, transform.rotation);
        Animator a = g.GetComponent<Animator>();
        float point = m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
        int currentAnimhash = m_Animator.GetCurrentAnimatorStateInfo(0).shortNameHash;
        float AxisX = m_Animator.GetFloat(m_AnimKeyMoveDirectionX);
        float AxisY = m_Animator.GetFloat(m_AnimKeyMoveDirectionY);
        float DAxisX = m_Animator.GetFloat(PlayerDashComponent.m_DashAxisKeyX);
        float DAxisY = m_Animator.GetFloat(PlayerDashComponent.m_DashAxisKeyY);
        a.SetFloat(m_AnimKeyMoveDirectionX, AxisX);
        a.SetFloat(m_AnimKeyMoveDirectionY, AxisY);
        a.SetFloat(PlayerDashComponent.m_DashAxisKeyX, DAxisX);
        a.SetFloat(PlayerDashComponent.m_DashAxisKeyY, DAxisY);
        a.CrossFade(currentAnimhash, 0.0f, 0, point);
        a.speed = 0.00f;

        Material spectrumMat = new Material(_Material);
        Renderer[] renderers = g.GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; ++i)
        {
            Material[] tmp = renderers[i].materials;
            for (int j = 0; j < tmp.Length; ++j)
            {
                tmp[j] = spectrumMat;
            }
            renderers[i].materials = tmp;

            foreach (ParticleSystem p in renderers[i].GetComponentsInChildren<ParticleSystem>())
            {
                Destroy(p.gameObject);
            }
        }
        CharacterSpectrumEffect specEffect = a.gameObject.AddComponent<CharacterSpectrumEffect>();
        specEffect.SetSpectrum(spectrumMat, _Duration, _AlphaDecrease);
    }
}
