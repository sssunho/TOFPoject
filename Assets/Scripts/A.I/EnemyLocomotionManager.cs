using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimationManager enemyAnimationManager;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        CharacterStats currentTarget;
        public LayerMask detectionLayer;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        }

        //public void HandleDetection()
        //{
        //    Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

        //    for(int i = 0; i < colliders.Length; i++)
        //    {
        //        CharacterStats characterStats = colliders[i].transform.GetComponent<CharacterStats>();

        //        if(characterStats != null)
        //        {
        //            Vector3 targetDirection = characterStats.transform.position - transform.position;
        //            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

        //            if(viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
        //            {
        //                currentTarget = characterStats;
        //            }
        //        }
        //    }
        //}
    }
}