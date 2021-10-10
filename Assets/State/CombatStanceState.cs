using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            if (enemyManager.isPerformingAction)
            {
                enemyAnimationManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
            }

            // #. If in attack range return attack State
            if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return attackState;
            }
            else if(distanceFromTarget > enemyManager.maximumAttackRange)
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}

