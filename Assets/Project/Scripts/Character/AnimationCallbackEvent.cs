using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCallbackEvent : MonoBehaviour
{
    CharacterBase m_Character;

    Dictionary<int, System.Action> m_AttackHitCallbackDic = new Dictionary<int, System.Action>();

    private void Awake()
    {
        m_Character = GetComponentInParent<CharacterBase>();
    }

    // 0 번은 default
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

    public void MotionEnd(int _Value)
    {
        m_Character.MotionEnd(_Value);
    }

    public void DashEnd()
    {
        m_Character.m_Rigidbody.velocity = Vector3.zero;
        m_Character.m_IsDashing = false;
    }
}
