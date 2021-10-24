using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolControl : StateMachineBehaviour
{
    public string boolName;

    bool initialized = false;
    int hash;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!initialized) hash = Animator.StringToHash(boolName);
        animator.SetBool(hash, true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(hash, false);
    }
}
