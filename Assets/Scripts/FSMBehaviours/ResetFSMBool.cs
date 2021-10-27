using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetFSMBool : StateMachineBehaviour
{
    public string name;

    bool init = false;
    int hash;

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        if (!init)
        {
            init = true;
            hash = Animator.StringToHash(name);
        }

        animator.SetBool(hash, false);
    }
}
