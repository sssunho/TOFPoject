using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunhoTestScript : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(animator.applyRootMotion);
    }
}
