using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDragonNormalAttackComponent : FireDraonBaseComponent
{
    static readonly int m_AnimFrontAttack1 = Animator.StringToHash("HeadAttackFront");
    static readonly int m_AnimFrontAttack2 = Animator.StringToHash("HeadAttackLeft");
    static readonly int m_AnimFrontAttack3 = Animator.StringToHash("HeadAttackRight");

    static readonly int m_AnimFrontTailAttack1 = Animator.StringToHash("TailAttackLeft");
    static readonly int m_AnimFrontTailAttack2 = Animator.StringToHash("TailAttackRight");

    readonly string m_TailAttackSmokeEffectPrefab = "DragonTailAttackSmoke";

    public float m_Cooldown = 5.0f;
    public float m_CooldownTimer = 1.0f;

    public float m_MaxDistance = 5.5f;
    public float m_MinDistance = 3;

    public override void Initialize(CharacterBase _CharacterBase)
    {
        base.Initialize(_CharacterBase);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(1,2,3, FrontHeadHitEvent);
        m_CharacterBase.m_AnimCallback.AddAttackHitEvent(4,5, FrontTailHitEvent);
        m_CharacterBase.m_AnimCallback.AddMotionStartEvent(4,5, FrontTailStart);
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
        if (!m_AICharacter.m_TargetCharacter) return;

        Vector3 direction = m_AICharacter.m_TargetCharacter.transform.position - m_AICharacter.transform.position;
        float distance = direction.magnitude;
        float angleDot = Vector3.Dot(m_AICharacter.transform.forward, direction.normalized);

        if (angleDot < 0.0f) // 뒤쪽일때의 로직
        {
            if (!BackAttack(direction, distance, angleDot)) return;
        }
        else // 앞쪽일때의 로직
        {
            if (!FrontAttack(direction, distance, angleDot)) return;
        }

        StartNewMotion();
        m_CooldownTimer = m_Cooldown;
    }

    bool FrontAttack(Vector3 _Direction, float _Distance, float _AngleDot)
    {
        if (_Distance >= m_MinDistance && _Distance <= m_MaxDistance)
        {
            if (_Distance > m_MaxDistance * 0.75f)
            {
                m_CharacterBase.m_Animator.CrossFade(m_AnimFrontAttack1, 0.15f);
            }
            else
            {
                int number = Random.Range(0, 100);
                if (number > 50)
                    m_CharacterBase.m_Animator.CrossFade(m_AnimFrontAttack2, 0.15f);
                else
                    m_CharacterBase.m_Animator.CrossFade(m_AnimFrontAttack3, 0.15f);
            }
            return true;
        }
        else if (_Distance < m_MinDistance)
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
        if (HitDamage(pos, 2, 0.75f))
        {
            Debug.Log("Dragon Head Hit");
        }
    }

    void FrontTailStart()
    {
        SoundManager.Instance.PlayDefaultSound(m_CharacterBase.m_AudioList[2], 0.5f);
        for (int i = 0; i < m_FireDragonCharacter.m_TailPoint.Length; ++i)
        {
            LifeTimerWithObjectPool life = ObjectPool.GetObject<LifeTimerWithObjectPool>(m_TailAttackSmokeEffectPrefab);
            life.Initialize();
            life.SetTargetTransform(m_FireDragonCharacter.m_TailPoint[i]);
            life.gameObject.SetActive(true);
        }
    }

    void FrontTailHitEvent()
    {

        Vector3 pos = m_FireDragonCharacter.transform.position;
        if (HitDamage(pos, 5, 6.5f))
        {
            Debug.Log("Dragon Tail Hit");
        }
    }

    bool BackAttack(Vector3 _Direction, float _Distance, float _AngleDot)
    {
        return true;
    }
}
