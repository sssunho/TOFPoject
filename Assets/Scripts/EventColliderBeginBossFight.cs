using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  TOF
{
    public class EventColliderBeginBossFight : MonoBehaviour
    {
        WorldEventManager worldEventManager;
        CameraHandler cameraHandler;
        public EnemyBossManager enemyBossManager;
        public FogWall fogWall;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player")
            {
                worldEventManager.bossNum = enemyBossManager._bossNum;
                enemyBossManager.SetBossInfo();
                worldEventManager.ActivateBossFight();
                cameraHandler.NormalShot();
                fogWall.FogPassed();
            }
        }
    }
}

