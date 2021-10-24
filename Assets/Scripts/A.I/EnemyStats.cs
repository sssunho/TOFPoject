using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class EnemyStats : CharacterStats
    {
        EnemyBossManager enemyBossManager;
        public UIEnemyHealthBar enemyHealthBar;

        public int soulsAwardedOnDeath = 50;

        public bool isBoss;

        private void Awake()
        {
            animatorManager = GetComponentInChildren<EnemyAnimationManager>();
            characterManager = GetComponent<EnemyManager>();
            enemyBossManager = GetComponent<EnemyBossManager>();
            maxHealth = SetMaxHealthFromHealthLevel();
            currentHealth = maxHealth;
        }

        private void Start()
        {
            if(!isBoss)
                enemyHealthBar.SetMaxHealth(maxHealth);
        }

        private int SetMaxHealthFromHealthLevel()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamageNoAnimation(int damage)
        {
            currentHealth = currentHealth - damage;

            enemyHealthBar.SetHealth(currentHealth);

            if (currentHealth <= 0)
            {
                currentHealth = 0;
                isDead = true;
            }
        }

        public override void TakeDamage(Damage damage)
        {
            base.TakeDamage(damage);

            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth);
            }

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        public void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (isDead)
                return;

            currentHealth = currentHealth - damage;
            animatorManager.PlayTargetAnimation(damageAnimation, true);
            if (!isBoss)
            {
                enemyHealthBar.SetHealth(currentHealth);
            }
            else if (isBoss && enemyBossManager != null)
            {
                enemyBossManager.UpdateBossHealthBar(currentHealth);
            }

            if (currentHealth <= 0)
            {
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            currentHealth = 0;
            animatorManager.PlayTargetAnimation("Dead_01", true);
            isDead = true;
        }
    }

}
