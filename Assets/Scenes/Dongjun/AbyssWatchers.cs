using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class AbyssWatchers : EnemyBossManager
    {
        int num = 1;
        string bossName = "AbyssWatchers";

        EnemyStats enemyStats;

        private void Awake()
        {
            enemyStats = GetComponent<EnemyStats>();
        }

        private void Start()
        {
            enemyStats.isBoss = true;
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
            bossHealthBar.SetBossName(bossName);
            _bossNum = num;
        }
    }
}



