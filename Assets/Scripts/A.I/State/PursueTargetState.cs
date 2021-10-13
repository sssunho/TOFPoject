using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PursueTargetState : State
    {
        public CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if (enemyManager.isInteracting)
                return this;
            // #.1 Chase the target
            if (enemyManager.isPerformingAction)
            {
                enemyAnimationManager.anim.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                return this;
            }

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            // if we are performing an action, stop our movement
            if (distanceFromTarget > enemyManager.maximumAttackRange)
            {
                enemyAnimationManager.anim.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotateTowardsTarget(enemyManager);

            // #.2 If within attack range, switch to combat stance state
            if (distanceFromTarget <= enemyManager.maximumAttackRange)
            {
                return combatStanceState;
            }
            // #.3 If target is out of range, return  this state and continue to chase target
            else
            {
                return this;
            }
        }

        private void HandleRotateTowardsTarget(EnemyManager enemyManager)
        {
            // Rotate manually
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, enemyManager.rotationSpeed);
            }
            // Rotate with pathfinding (navmesh)
            else
            {
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);

                enemyManager.navMeshAgent.updateRotation = false;
                enemyManager.navMeshAgent.updatePosition = false;

                Vector3 targetVelocity = enemyManager.navMeshAgent.desiredVelocity;
                Vector3 lookPos = enemyManager.currentTarget.transform.position - transform.position;
                lookPos.y = 0;
                Quaternion targetRot = Quaternion.LookRotation(lookPos);

                enemyManager.controller.gameObject.transform.rotation = Quaternion.Slerp(enemyManager.controller.gameObject.transform.rotation, targetRot, Time.deltaTime * 3.0f);

                enemyManager.controller.Move(targetVelocity * Time.deltaTime);
                enemyManager.navMeshAgent.velocity = enemyManager.controller.velocity;
            }
        }
    }
}

