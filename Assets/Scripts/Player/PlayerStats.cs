using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class PlayerStats : CharacterStats
    {
        PlayerManager playerManager;
        GameManager gameManager;
        public SliderControl healthbar;
        public SliderControl staminabar;
        public SliderControl focusPointBar;
        public SliderControl strengthbar;

        public float staminaRegenerationAmount = 1;
        public float staminaRegenTimer;

        PlayerAnimationManager playerAnimationHandler;

        private void Awake()
        {
            playerManager = GetComponent<PlayerManager>();
            playerAnimationHandler = GetComponentInChildren<PlayerAnimationManager>();
            gameManager = FindObjectOfType<GameManager>();
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
        public void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (playerManager.isInvulnerable) return;

            if (isDead) return;

            currentHealth -= damage;
            healthbar.setCurValue(currentHealth);

            playerAnimationHandler.PlayTargetAnimation(damageAnimation, true);

            if(currentHealth<=0)
            {
                currentHealth = 0;
                playerAnimationHandler.PlayTargetAnimation("Dead_01", true);
                isDead = true;
                gameManager.OnCharacterDead(this.gameObject);
            }
        }

        public void TakeStaminaDamage(int damage)
        {
            currentStamina -= damage;
            if(currentStamina <= 0)
            {
                currentStamina = 0;
            }
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
                if (currentStamina > maxStamina)
                    currentStamina = maxStamina;
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

        public void AddSouls(int souls)
        {
            soulCount += souls;
        }
    }
}
