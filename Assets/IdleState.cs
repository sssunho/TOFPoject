using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class IdleState : State
    {
        public PursueTargetState pursueTargetState;
        public LayerMask detectionLayer;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {

            // #.1 Look for a potential target
            #region Handle Enemy Target Detection
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

                if (characterStats != null)
                {
                    // CHECK FOR TEAM ID
                    Vector3 targetDirection = characterStats.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        enemyManager.currentTarget = characterStats;
                        return pursueTargetState;
                    }
                }
            }
            #endregion
            
            // #.2 Switch to the pursue target state if target is found
            #region Handle Switching To Next State
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }
            // #.3 If not return this state
            else
            {
                return this;
            }
            #endregion
        }
    }
}

