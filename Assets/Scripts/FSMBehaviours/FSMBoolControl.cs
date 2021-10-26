using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMBoolControl : StateMachineBehaviour
{
    public string boolName;

    bool initialized = false;
    int hash;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (!initialized)
        {
            initialized = true;
            hash = Animator.StringToHash(boolName);
        }
        animator.SetBool(hash, true);
    }
    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        animator.SetBool(hash, false);
    }

}
