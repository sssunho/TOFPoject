using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class AbyssWatchers : EnemyBossManager
    {
        int num = 1;
        private string name = "AbyssWatchers";

        EnemyStats enemyStats;

        private void Awake()
        {
            enemyStats = GetComponent<EnemyStats>();
        }

        public override void SetBossInfo()
        {
            enemyStats.isBoss = true;
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
            bossHealthBar.SetBossName(name);
            _bossNum = num;
        }
    }
}