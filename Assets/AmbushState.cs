using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class AmbushState : State
    {
        public bool isSleeping;
        public float detectionRadius = 2;
        public string sleepAnimation;
        LayerMask detectionLayer;

        public PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStats enemyStats, EnemyAnimationManager enemyAnimationManager)
        {
            if(isSleeping && enemyManager.isPerformingAction == false)
            {
                enemyAnimationManager.PlayTargetAnimation(sleepAnimation, true);
            }

            #region Handle Target Detection
            Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, detectionLayer);

            for(int i = 0; i < colliders.Length; i++)
            {
                PlayerManager playerManager = colliders[i].transform.GetComponent<PlayerManager>();

            }
            return this;
            #endregion
        }
    }
}

