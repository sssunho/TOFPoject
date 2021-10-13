using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class RotateTowardsTargetState : State
    {
        public CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            enemyAnimationManager.anim.SetFloat("Vertical", 0);
            enemyAnimationManager.anim.SetFloat("Horizontal", 0);

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float viewableAngle = Vector3.SignedAngle(targetDirection, enemyManager.transform.forward, Vector3.up);

            if (enemyManager.isInteracting)
                return this;

            if(viewableAngle >= 100 && viewableAngle <= 180 && !enemyManager.isInteracting)
            {
                enemyAnimationManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -101 && viewableAngle >= -180 && !enemyManager.isInteracting)
            {
                enemyAnimationManager.PlayTargetAnimationWithRootRotation("Turn Behind", true);
                return combatStanceState;
            }
            else if (viewableAngle <= -45 && viewableAngle >= -100 && !enemyManager.isInteracting)
            {
                enemyAnimationManager.PlayTargetAnimationWithRootRotation("Turn Right", true);
                return combatStanceState;
            }
            else if (viewableAngle >= 45 && viewableAngle <= 100 && !enemyManager.isInteracting)
            {
                enemyAnimationManager.PlayTargetAnimationWithRootRotation("Turn Left", true);
                return combatStanceState;
            }

            return combatStanceState;
        }
    }
}
