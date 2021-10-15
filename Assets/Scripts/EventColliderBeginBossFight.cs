using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  TOF
{
    public class EventColliderBeginBossFight : MonoBehaviour
    {
        WorldEventManager worldEventManager;
        public EnemyBossManager enemyBossManager;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                worldEventManager.bossNum = enemyBossManager._bossNum;
                enemyBossManager.SetBossInfo();
                worldEventManager.ActivateBossFight();
            }
        }
    }
}

