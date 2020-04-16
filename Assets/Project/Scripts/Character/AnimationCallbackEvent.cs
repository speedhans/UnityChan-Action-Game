using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallbackEvent : MonoBehaviour
{
    CharacterBase m_Character;

    Dictionary<int, System.Action> m_MotionStartCallbackDic = new Dictionary<int, System.Action>();
    Dictionary<int, System.Action> m_MotionEndCallbackDic = new Dictionary<int, System.Action>();
    Dictionary<int, System.Action> m_AttackHitCallbackDic = new Dictionary<int, System.Action>();


    private void Awake()
    {
        m_Character = GetComponentInParent<CharacterBase>();
    }

    // 0 번은 default

    public bool AddAttackHitEvent(System.Action _Function, params int[] _Number)
    {
        bool result = true;

        for (int i = 0; i < _Number.Length; ++i)
        {
            if (!AddAttackHitEvent(_Function, _Number[i]))
            {
                for (int j = i; j > -1; --j)
                {
                    ReleaseAttackHitEvent(_Number[j]);
                }
                result = false;
            }
        }

        return result;
    }

    public bool AddAttackHitEvent(System.Action _Function, int _Number)
    {
        System.Action action = null;
        if (m_AttackHitCallbackDic.TryGetValue(_Number, out action))
        {
            return false;
        }

        m_AttackHitCallbackDic.Add(_Number, _Function);

        return true;
    }

    public void ReleaseAttackHitEvent(params int[] _Number)
    {
        for (int i = 0; i < _Number.Length; ++i)
        {
            m_AttackHitCallbackDic.Remove(_Number[i]);
        }
    }

    public bool AddMotionStartEvent(System.Action _Function, params int[] _Number)
    {
        bool result = true;

        for (int i = 0; i < _Number.Length; ++i)
        {
            if (!AddMotionStartEvent(_Function, _Number[i]))
            {
                for (int j = i; j > -1; --j)
                {
                    ReleaseMotionStartEvent(_Number[j]);
                }
                result = false;
            }
        }

        return result;
    }
    public bool AddMotionStartEvent(System.Action _Function, int _Number)
    {
        System.Action action = null;
        if (m_MotionStartCallbackDic.TryGetValue(_Number, out action))
        {
            return false;
        }

        m_MotionStartCallbackDic.Add(_Number, _Function);

        return true;
    }

    public void ReleaseMotionStartEvent(params int[] _Number)
    {
        for (int i = 0; i < _Number.Length; ++i)
        {
            m_MotionStartCallbackDic.Remove(_Number[i]);
        }
    }

    public bool AddMotionEndEvent(System.Action _Function, params int[] _Number)
    {
        bool result = true;

        for (int i = 0; i < _Number.Length; ++i)
        {
            if (!AddMotionEndEvent(_Function, _Number[i]))
            {
                for (int j = i; j > -1; --j)
                {
                    ReleaseMotionEndEvent(_Number[j]);
                }
                result = false;
            }
        }

        return result;
    }
    public bool AddMotionEndEvent(System.Action _Function, int _Number)
    {
        System.Action action = null;
        if (m_MotionEndCallbackDic.TryGetValue(_Number, out action))
        {
            return false;
        }

        m_MotionEndCallbackDic.Add(_Number, _Function);

        return true;
    }

    public void ReleaseMotionEndEvent(params int[] _Number)
    {
        for (int i = 0; i < _Number.Length; ++i)
        {
            m_MotionEndCallbackDic.Remove(_Number[i]);
        }
    }

    public void FootL()
    {
        if (m_Character.IsAI) return;
        SoundManager.Instance.PlayDefaultSound(GameManager.Instacne.m_Main.m_FootClip1, 0.5f);
    }

    public void FootR()
    {
        if (m_Character.IsAI) return;
        SoundManager.Instance.PlayDefaultSound(GameManager.Instacne.m_Main.m_FootClip1, 0.5f);
    }

    public void AttackHit(int _Value)
    {
        System.Action action = null;
        if (m_AttackHitCallbackDic.TryGetValue(_Value, out action))
        {
            action.Invoke();
        }
    }

    public void MotionStart(int _Value)
    {
        System.Action action = null;
        if (m_MotionStartCallbackDic.TryGetValue(_Value, out action))
        {
            action.Invoke();
        }
    }

    public void MotionEnd(int _Value)
    {
        System.Action action = null;
        if (m_MotionEndCallbackDic.TryGetValue(_Value, out action))
        {
            action.Invoke();
        }

        m_Character.MotionEnd(_Value);
    }

    public void DashEnd()
    {
        m_Character.m_Rigidbody.velocity = Vector3.zero;
        m_Character.m_IsDashing = false;
    }

    public void HitEnd(int _Value)
    {
        m_Character.HitEnd(_Value);
    }
}
