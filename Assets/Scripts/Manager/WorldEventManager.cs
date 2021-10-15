using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class WorldEventManager : MonoBehaviour
    {
        public int bossNum;
        public List<EnemyBossManager> bosses;
        public List<FogWall> fogWalls;
        UIBossHealthBar bossHealthBar;

        public bool bossFightIsActive;
        public bool bossHasBeenAwaked;
        public bool bossHasBeenDefeated;

        private void Awake()
        {
            bossHealthBar = FindObjectOfType<UIBossHealthBar>();
        }

        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            bossHasBeenAwaked = true;
            bossHealthBar.SetUIHealthBarToActive();

            if (bosses[bossNum].isFirstTry) bosses[bossNum].isFirstTry = false;
            for (int i = 0; i < fogWalls.Count; i++)
            {
                Debug.Log(bosses[i].isFirstTry);
                if (!bosses[i].bossDeafeted)
                    fogWalls[i].ActivateFogWall();
            }
        }

        public void BossHasBeenDefeated()
        {
            bossHasBeenDefeated = true;
            bossFightIsActive = false;

            for (int i = 0; i < fogWalls.Count; i++)
            {
                if (bosses[i].bossDeafeted) 
                    fogWalls[i].DeactivateFogWall();
            }
        }
    }

    
}

