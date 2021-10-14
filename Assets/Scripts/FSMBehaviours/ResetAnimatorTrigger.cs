using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetAnimatorTrigger : StateMachineBehaviour
{
    public bool onEnter;
    public bool onExit;
    public string trigger;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onEnter)
            animator.ResetTrigger(trigger);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (onExit)
            animator.ResetTrigger(trigger);
    }
}
