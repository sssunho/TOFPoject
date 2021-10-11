using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class PlayerStats : CharacterStats
    {
        PlayerManager playerManager;

        public SliderControl healthbar;
        public SliderControl staminabar;
        public SliderControl focusPointBar;
        public SliderControl strengthbar;

        public float staminaRegenerationAmount = 1;
        public float staminaRegenTimer;

        AnimatorHandler animatorHandler;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
        }

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLV();
            maxStamina = SetMaxStaminaFromHealthLV();
            maxFocusPoints = SetMaxFocusPointsFromFocusLevel();
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentFocusPoint = maxFocusPoints;
            healthbar.setMaxValue(maxHealth);
            staminabar.setMaxValue(maxStamina);
            focusPointBar.setMaxValue(maxFocusPoints);
            focusPointBar.setCurValue(currentFocusPoint);
        }

        private int SetMaxHealthFromHealthLV()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private float SetMaxStaminaFromHealthLV()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
        }

        private float SetMaxFocusPointsFromFocusLevel()
        {
            maxFocusPoints = focusLevel * 10;
            return maxFocusPoints;
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
        public void TakeDamage(int damage)
        {
            if (playerManager.isInvulnerable)
                return;

            if (isDead)
                return;

            currentHealth -= damage;
            healthbar.setCurValue(currentHealth);

            animatorHandler.PlayTargetAnimation("Damage_01", true);

            if(currentHealth<=0)
            {
                currentHealth = 0;
                animatorHandler.PlayTargetAnimation("Dead_01", true);
                isDead = true;
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina -= damage;
            staminabar.setCurValue(currentStamina);
        }

        public void RegenerateStamina()
        {
            if (playerManager.isInteracting)
            {
                staminaRegenTimer = 0;
                return;
            }

            staminaRegenTimer += Time.deltaTime;

            if (currentStamina < maxStamina && staminaRegenTimer > 1.0f)
            {
                currentStamina += staminaRegenerationAmount + Time.deltaTime;
                staminabar.setCurValue(Mathf.RoundToInt(currentStamina));
            }
        }

        public void HealPlayer(int healAmount)
        {
            currentHealth += healAmount;
            if(currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            healthbar.setCurValue(currentHealth);
        }

        public void DeductFocusPoints(int focusPoints)
        {
            currentFocusPoint -= focusPoints;
            if(currentFocusPoint < 0)
            {
                currentFocusPoint = 0;
            }
            focusPointBar.setCurValue(currentFocusPoint);
        }
    }
}