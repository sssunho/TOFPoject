using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class EnemyLocomotionManager : MonoBehaviour
    {
        EnemyManager enemyManager;
        EnemyAnimationManager enemyAnimationManager;

        private void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        }
    }
}