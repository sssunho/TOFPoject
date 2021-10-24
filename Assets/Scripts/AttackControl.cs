using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class AttackControl : StateMachineBehaviour
    {
        public float damageModifier = 1.0f;
        public HitReaction reaction = HitReaction.NONE;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat("DamageModifier", damageModifier);
            animator.GetComponentInChildren<DamageCollider>().hitReaction = reaction;
        }
    }
}