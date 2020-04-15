using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallbackEvent : MonoBehaviour
{
    CharacterBase m_Character;

    Dictionary<int, System.Action> m_MotionStartCallbackDic = new Dictionary<int, System.Action>();
    Dictionary<int, System.Action> m_AttackHitCallbackDic = new Dictionary<int, System.Action>();


    private void Awake()
    {
        m_Character = GetComponentInParent<CharacterBase>();
    }

    // 0 번은 default

    public bool AddAttackHitEvent(int _Number1, int _Number2, System.Action _Function)
    {
        bool result = true;
        if (!AddAttackHitEvent(_Number1, _Function)) result = false;
        if (!AddAttackHitEvent(_Number2, _Function))
        {
            ReleaseAttackHitEvent(_Number1);
            result = false;
        }

        return result;
    }

    public bool AddAttackHitEvent(int _Number1, int _Number2, int _Number3, System.Action _Function)
    {
        bool result = true;
        if (!AddAttackHitEvent(_Number1, _Function)) result = false;
        if (!AddAttackHitEvent(_Number2, _Function))
        {
            ReleaseAttackHitEvent(_Number1);
            result = false;
        }
        if (!AddAttackHitEvent(_Number3, _Function))
        {
            ReleaseAttackHitEvent(_Number2);
            ReleaseAttackHitEvent(_Number1);
            result = false;
        }

        return result;
    }

    public bool AddAttackHitEvent(int _Number, System.Action _Function)
    {
        System.Action action = null;
        if (m_AttackHitCallbackDic.TryGetValue(_Number, out action))
        {
            return false;
        }

        m_AttackHitCallbackDic.Add(_Number, _Function);

        return true;
    }

    public void ReleaseAttackHitEvent(int _Number)
    {
        m_AttackHitCallbackDic.Remove(_Number);
    }

    public bool AddMotionStartEvent(int _Number1, int _Number2, System.Action _Function)
    {
        bool result = true;
        if (!AddMotionStartEvent(_Number1, _Function)) result = false;
        if (!AddMotionStartEvent(_Number2, _Function))
        {
            ReleaseAttackHitEvent(_Number1);
            result = false;
        }

        return result;
    }

    public bool AddMotionStartEvent(int _Number1, int _Number2, int _Number3, System.Action _Function)
    {
        bool result = true;
        if (!AddMotionStartEvent(_Number1, _Function)) result = false;
        if (!AddMotionStartEvent(_Number2, _Function))
        {
            ReleaseMotionStartEvent(_Number1);
            result = false;
        }
        if (!AddMotionStartEvent(_Number3, _Function))
        {
            ReleaseAttackHitEvent(_Number2);
            ReleaseAttackHitEvent(_Number1);
            result = false;
        }

        return result;
    }

    public bool AddMotionStartEvent(int _Number, System.Action _Function)
    {
        System.Action action = null;
        if (m_MotionStartCallbackDic.TryGetValue(_Number, out action))
        {
            return false;
        }

        m_MotionStartCallbackDic.Add(_Number, _Function);

        return true;
    }

    public void ReleaseMotionStartEvent(int _Number)
    {
        m_MotionStartCallbackDic.Remove(_Number);
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
