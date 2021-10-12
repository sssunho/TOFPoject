using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineTest : StateMachineBehaviour
{
    float time = 0;
    int i = 0;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        i++;
        time += Time.deltaTime;
        Debug.Log(i / time);
    }
}
