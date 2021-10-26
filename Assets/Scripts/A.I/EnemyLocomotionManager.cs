using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimationManager enemyAnimationManager;
        public bool ignoreGravity = false;

        public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        public LayerMask detectionLayer;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        }

        private void Start()
        {
            //Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }

        private void Update()
        {
            if (!ignoreGravity && !enemyManager.isInteracting)
                enemyManager.controller.Move(-9.81f * Vector3.up * Time.deltaTime);

        }
    }
}