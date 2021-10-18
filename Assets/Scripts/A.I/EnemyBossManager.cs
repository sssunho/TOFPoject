using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class EnemyBossManager : MonoBehaviour
    {
        protected WorldEventManager worldEventManager;
        public UIBossHealthBar bossHealthBar;

        public int _bossNum;
        public bool bossDeafeted;
        public bool isFirstTry;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        public void UpdateBossHealthBar(int currentHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);
        }

        public virtual void SetBossInfo()
        {
            
        }
    }
}