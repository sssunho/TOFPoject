using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TOF
{
    public class EnemyManager : CharacterManager
    {
        EnemyLocomotionManager enemyLocomotionManager;
        EnemyAnimationManager enemyAnimationManager;
        EnemyStats enemyStats;

        public State currentState;
        public CharacterStats currentTarget;
        public Rigidbody enemyRigidBody;
        public NavMeshAgent navMeshAgent;
        public CharacterController controller;

        public bool isPerformingAction;
        public float rotationSpeed = 15;
        public float maximumAggroRadius = 1.5f;

        [Header("Combat Flags")]
        public bool canDoCombo;

        [Header("A.I Settings")]
        public float detectionRadius = 15;
        // The higher, and lower, respectively these angles are, the greater detection FIELD OF VIEW (basically like eye sight)
        public float maximumDetectionAngle = 50;
        public float minimumDetectionAngle = -50;
        public float currentRecoveryTime = 0;

        [Header("A.I Combat Settings")]
        public bool allowAIToPerformCombos;
        public float comboLikelyHood;

        private void Awake()
        {
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
            enemyStats = GetComponent<EnemyStats>();
            enemyRigidBody = GetComponent<Rigidbody>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            controller = GetComponent<CharacterController>();
            navMeshAgent.enabled = false;
        }

        private void Start()
        {
            navMeshAgent.updateRotation = false;
            navMeshAgent.updatePosition = false;
        }

        private void Update()
        {
            isRotatingWithRootMotion = enemyAnimationManager.anim.GetBool("isRotatingWithRootMotion");
            isInteracting = enemyAnimationManager.anim.GetBool("isInteracting");
            isBlocking = enemyAnimationManager.anim.GetBool("isBlocking");
            canDoCombo = enemyAnimationManager.anim.GetBool("canDoCombo");
            canRotate = enemyAnimationManager.anim.GetBool("canRotate");
            enemyAnimationManager.anim.SetBool("isDead", enemyStats.isDead);
        }
        
        private void HandleStateMachine()
        {
            if(currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStats, enemyAnimationManager);

                if(nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State state)
        {
            currentState = state;
        }

        private void HandleRecoveryTimer()
        {
            if(currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if(isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                    isPerformingAction = false;
            }
        }


    }
}

