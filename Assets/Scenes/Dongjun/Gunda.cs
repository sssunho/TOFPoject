using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class Gunda : EnemyBossManager
    {
        int num = 0;
        string bossName = "Gunda";

        EnemyStats enemyStats;

        private void Awake()
        {
            enemyStats = GetComponent<EnemyStats>();
        }

        public void SetBossInfo()
        {
            enemyStats.isBoss = true;
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
            bossHealthBar.SetBossName(bossName);
            _bossNum = num;
        }
    }
}