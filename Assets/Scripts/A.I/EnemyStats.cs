using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class EnemyStats : CharacterStats
    {
        EnemyAnimationManager enemyAnimationManager;

        public int soulsAwardedOnDeath = 50;

        private void Awake()
        {
            enemyAnimationManager = GetComponentInChildren<EnemyAnimationManager>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (isDead)
                return;
            currentHealth = currentHealth - damage;

            enemyAnimationManager.PlayTargetAnimation(damageAnimation, true);

            if(currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            enemyAnimationManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }
    }

}
