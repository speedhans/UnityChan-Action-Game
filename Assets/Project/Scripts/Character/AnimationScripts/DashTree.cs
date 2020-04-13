using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTree : StateMachineBehaviour
{
    bool m_Enable = false;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_Enable = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime < 0.8f) return;
        if (!m_Enable) return;
        m_Enable = false;
        AnimationCallbackEvent callback = animator.GetComponent<AnimationCallbackEvent>();
        callback.MotionEnd(0);
        callback.DashEnd();
    }
}
