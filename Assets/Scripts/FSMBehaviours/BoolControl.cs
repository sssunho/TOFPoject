using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolControl : StateMachineBehaviour
{
    public string boolName;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(boolName, false);
    }
}
