using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class WorldEventManager : MonoBehaviour
    {
        public List<FogWall> fogWalls;

        public bool bossFightIsActive;
        public bool bossHasBeenAwaked;
        public bool bossHasBeenDefeated;

        public void ActivateBossFight()
        {
            bossFightIsActive = true;
            bossHasBeenAwaked = true;

            foreach (var fogwall in fogWalls)
            {
                fogwall.ActivateFogWall();
            }
        }

        public void BossHasBeenDefeated()
        {
            bossHasBeenDefeated = true;
            bossFightIsActive = false;

            foreach (var fogwall in fogWalls)
            {
                fogwall.DeactivateFogWall();
            }
        }
    }

    
}

