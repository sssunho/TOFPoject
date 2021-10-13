using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class CombatStanceState : State
    {
        public AttackState attackState;
        public EnemyAttackAction[] enemyAttacks;
        public PursueTargetState pursueTargetState;

        bool randomDestinationSet = false;
        float verticalMovementValue = 0;
        float horizontalMovementValue = 0;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            enemyAnimationManager.anim.SetFloat("Vertical", verticalMovementValue, 0.2f, Time.deltaTime);
            enemyAnimationManager.anim.SetFloat("Horizontal", horizontalMovementValue, 0.2f, Time.deltaTime);
            attackState.hasPerformedAttack = false;
            if (enemyManager.isInteracting)
            {
                enemyAnimationManager.anim.SetFloat("Vertical", 0);
                enemyAnimationManager.anim.SetFloat("Horizontal", 0);
                return this;
            }

            else if (distanceFromTarget > enemyManager.maximumAggroRadius)
            {
                return pursueTargetState;
            }

            if(!randomDestinationSet)
            {
                randomDestinationSet = true;
                // decide circling action
                DecideCircleingAction(enemyAnimationManager);
            }

            HandleRotateTowardsTarget(enemyManager);

            // #. If in attack range return attack State
            if (enemyManager.currentRecoveryTime <= 0 && attackState.currentAttack != null)
            {
                randomDestinationSet = false;
                return attackState;
            }
            else
            {
                GetNewAttack(enemyManager);
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

        private void DecideCircleingAction(EnemyAnimationManager enemyAnimationManager)
        {
            // #. 1 Circle with only forawrd vertical movement
            // #. 2 Circle with running
            WalkAroundTarget(enemyAnimationManager);
        }

        private void WalkAroundTarget(EnemyAnimationManager enemyAnimationManager)
        {
            verticalMovementValue = 0.5f;

            horizontalMovementValue = Random.Range(-1, 1);

            if (horizontalMovementValue <= 1 && horizontalMovementValue >= 0)
            {
                horizontalMovementValue = 0.5f;
            }
            else if (horizontalMovementValue >= -1 && horizontalMovementValue < 0)
            {
                horizontalMovementValue = -0.5f;
            }
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            int maxScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        maxScore += enemyAttackAction.attackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            for (int i = 0; i < enemyAttacks.Length; i++)
            {
                EnemyAttackAction enemyAttackAction = enemyAttacks[i];

                if (distanceFromTarget <= enemyAttackAction.maximumDistanceNeededToAttack
                    && distanceFromTarget >= enemyAttackAction.minimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= enemyAttackAction.maximumAttackAngle
                        && viewableAngle >= enemyAttackAction.minimumAttackAngle)
                    {
                        if (attackState.currentAttack != null)
                            return;

                        temporaryScore += enemyAttackAction.attackScore;

                        if (temporaryScore > randomValue)
                        {
                            attackState.currentAttack = enemyAttackAction;
                        }
                    }
                }
            }
        }

    }
}

