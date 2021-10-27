using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TOF
{
    public class IgnoreGravity : StateMachineBehaviour
    {
        PlayerLocomotion locomotion = null;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (locomotion == null)
                animator.GetComponent<PlayerLocomotion>();

            if (locomotion != null)
                locomotion.ignoreGravity = true;
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if(locomotion != null)
            {
                locomotion.ignoreGravity = false;
            }
        }
    }

}