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

        public override void SetBossInfo()
        {
            enemyStats.isBoss = true;
            bossHealthBar.SetBossMaxHealth(enemyStats.maxHealth);
            bossHealthBar.SetBossName(name);
            _bossNum = num;
        }
    }
}