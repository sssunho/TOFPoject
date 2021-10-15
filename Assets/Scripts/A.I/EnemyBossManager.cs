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
        public bool bossDeafeted = false;
        public bool isFirstTry = true;

        private void Awake()
        {
            worldEventManager = FindObjectOfType<WorldEventManager>();
        }

        private void Start()
        {
            worldEventManager.bossNum = _bossNum;
        }

        public void UpdateBossHealthBar(int currentHealth)
        {
            bossHealthBar.SetBossCurrentHealth(currentHealth);
        }
    }
}