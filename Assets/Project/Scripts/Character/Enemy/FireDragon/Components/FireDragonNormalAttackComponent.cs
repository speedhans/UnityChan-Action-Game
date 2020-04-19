using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonNormalAttackComponent : FireDragonBaseComponent
{
    static readonly int m_AnimFrontAttack1 = Animator.StringToHash("HeadAttackFront");
    static readonly int m_AnimFrontAttack2 = Animator.StringToHash("HeadAttackLeft");
    static readonly int m_AnimFrontAttack3 = Animator.StringToHash("HeadAttackRight");

    static readonly int m_AnimFrontTailAttack1 = Animator.StringToHash("TailAttackLeft");
    static readonly int m_AnimFrontTailAttack2 = Animator.StringToHash("TailAttackRight");

    static readonly int m_AnimFrontWingAttack1 = Animator.StringToHash("WingAttackLeft");
    static readonly int m_AnimFrontWingAttack2 = Animator.StringToHash("WingAttackRight");

    readonly string m_TailAttackSmokeEffectPrefab = "DragonTailAttackSmoke";
    readonly string m_TailAttackHitEffectPrefab = "DragonTailHit";
    readonly string m_DragonHeadAttackHitPrefab = "DragonHeadAttackHit";
    readonly string m_DragonWingAttackSmokePrefab = "DragonWingAttackSmoke";

    public float m_Cooldown = 5.0f;
    public float m_CooldownTimer = 1.0f;

    public float m_MaxDistance = 5.5f;
    public float m_MinDistance = 3;

    bool m_TailAttack = false;

    float m_HeadAttackDamage = 10.0f;
    float m_WingAttackDamage = 12.0f;
    float m_TailAttackDamage = 15.0f;

    HashSet<CharacterBase> m_DamageList = new HashSet<CharacterBase>();

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(FrontHeadHitEvent, 1, 2, 3);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(FrontWingAttackLeft, 6);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(FrontWingAttackRight, 7);
        m_CharacterBase.m_AnimCallback.AddMotionStartEvent(FrontTailStart, 4, 5);
        m_CharacterBase.m_AnimCallback.AddMotionEndEvent(FrontTailEnd, 4, 5);
    }

    public override void UpdateComponent(float _DeltaTime)
    {
        base.UpdateComponent(_DeltaTime);

        if (m_CooldownTimer > 0.0f)
        {
            m_CooldownTimer -= _DeltaTime;
            return;
        }

        if (!DefaultStateCheck()) return;
        if (!DragonStateCheck()) return;
        if (!m_AICharacter.m_TargetCharacter) return;

        Vector3 direction = m_AICharacter.GetTargetData().Direction;
        float distance = m_AICharacter.GetTargetData().Distance;
        float angle = m_AICharacter.GetTargetData().AngleBetweenTarget;

        if (angle <= 90.0f) // 앞쪽일때의 로직
        {
            if (!FrontAttack(direction, distance, angle)) return;
        }
        else // 뒤쪽일때의 로직
        {
            if (!BackAttack(direction, distance, angle)) return;
            return;
        }

        StartNewMotion();
        m_CooldownTimer = m_Cooldown;
    }

    public override void FixedUpdateComponent(float _FixedDeltaTime)
    {
        base.FixedUpdateComponent(_FixedDeltaTime);

        if (m_TailAttack)
        {
            Vector3 dir = m_FireDragonCharacter.m_TailPoint[0].position - m_FireDragonCharacter.transform.position;
            dir.y = 0.0f;
            dir.Normalize();
            HashSet<CharacterBase> list = OverlabShape(m_FireDragonCharacter, m_FireDragonCharacter.transform.position, dir, 2.5f, 6.5f);
            foreach(CharacterBase c in list)
            {
                if (m_DamageList.Contains(c)) continue;
                c.GiveToDamage(m_FireDragonCharacter, m_TailAttackDamage, true);
                m_DamageList.Add(c);

                LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_TailAttackHitEffectPrefab);
                life.Initialize();
                life.transform.position = c.transform.position + Vector3.up;
                life.gameObject.SetActive(true);
            }
        }
    }

    bool FrontAttack(Vector3 _Direction, float _Distance, float _Angle)
    {
        if (_Distance >= m_MinDistance && _Distance <= m_MaxDistance)
        {
            if (_Distance > m_MaxDistance * 0.75f) // Head Attack
            {
                m_CharacterBase.m_Animator.CrossFade(m_AnimFrontAttack1, 0.15f);
            }
            else
            {
                int number = Random.Range(0, 100);
                if (number > 50) // Head Attack
                {
                    number = Random.Range(0, 100);
                    if (number > 50)
                        m_CharacterBase.m_Animator.CrossFade(m_AnimFrontAttack2, 0.15f);
                    else
                        m_CharacterBase.m_Animator.CrossFade(m_AnimFrontAttack3, 0.15f);
                }
                else // Wing Attack
                {
                    Vector3 dir = m_AICharacter.m_TargetCharacter.transform.position - m_CharacterBase.transform.position;
                    dir.y = 0.0f;
                    dir.Normalize();

                    float dot = Vector3.Dot(m_AICharacter.transform.right, dir);
                    if (dot > 0)
                        m_CharacterBase.m_Animator.CrossFade(m_AnimFrontWingAttack2, 0.15f);
                    else
                        m_CharacterBase.m_Animator.CrossFade(m_AnimFrontWingAttack1, 0.15f);
                }
            }
            return true;
        }
        else if (_Distance < m_MinDistance) // Tail Attack
        {
            float rightDot = Vector3.Dot(m_AICharacter.transform.right, _Direction.normalized);
            if (rightDot >= 0.0f)
            {
                m_CharacterBase.m_Animator.CrossFade(m_AnimFrontTailAttack2, 0.15f);
            }
            else
            {
                m_CharacterBase.m_Animator.CrossFade(m_AnimFrontTailAttack1, 0.15f);
            }

            return true;
        }

        return false;
    }

    void FrontHeadHitEvent()
    {
        if (Random.Range(0, 1) == 0)
            SoundManager.Instance.PlayDefaultSound(m_CharacterBase.m_AudioList[0], 0.5f);
        else
            SoundManager.Instance.PlayDefaultSound(m_CharacterBase.m_AudioList[1], 0.5f);

        Vector3 pos = m_FireDragonCharacter.m_HeadPoint.transform.position;
        if (HitDamage(m_CharacterBase, pos, m_HeadAttackDamage, 0.75f))
        {
            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_DragonHeadAttackHitPrefab);
            life.Initialize();
            life.transform.position = pos;
            life.gameObject.SetActive(true);
        }
    }

    void FrontTailStart()
    {
        m_DamageList.Clear();
        SoundManager.Instance.PlayDefaultSound(m_CharacterBase.m_AudioList[2], 0.5f);
        for (int i = 0; i < m_FireDragonCharacter.m_TailPoint.Length; ++i)
        {
            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_TailAttackSmokeEffectPrefab);
            life.Initialize();
            life.SetTargetTransform(m_FireDragonCharacter.m_TailPoint[i]);
            life.gameObject.SetActive(true);
        }
        m_TailAttack = true;
    }

    void FrontTailEnd()
    {
        m_TailAttack = false;
        m_DamageList.Clear();
    }

    void FrontWingAttackLeft()
    {
        HitDamage(m_FireDragonCharacter, m_FireDragonCharacter.m_LeftHandPoint.transform.position, m_WingAttackDamage, 1);
        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_DragonWingAttackSmokePrefab);
        life.Initialize();
        life.transform.position = m_FireDragonCharacter.m_LeftHandPoint.transform.position + Vector3.down * 0.5f;
        life.gameObject.SetActive(true);
    }

    void FrontWingAttackRight()
    {
        HitDamage(m_FireDragonCharacter, m_FireDragonCharacter.m_RightHandPoint.transform.position, m_WingAttackDamage, 1);
        LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_DragonWingAttackSmokePrefab);
        life.Initialize();
        life.transform.position = m_FireDragonCharacter.m_RightHandPoint.transform.position + Vector3.down * 0.5f;
        life.gameObject.SetActive(true);
    }

    bool BackAttack(Vector3 _Direction, float _Distance, float _AngleDot)
    {
        return true;
    }
}
