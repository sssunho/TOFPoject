using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunhoTestScript : StateMachineBehaviour
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("asdf");
    }
}
