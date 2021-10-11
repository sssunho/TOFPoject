using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class PlayerStats : CharacterStats
    {

        public SliderControl healthbar;
        public SliderControl staminabar;
        public SliderControl strengthbar;

        AnimatorHandler animatorHandler;

        private void Start()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();

            maxHealth = SetMaxHealthFromHealthLV();
            maxStamina = SetMaxStaminaFromHealthLV();
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            healthbar.setMaxValue(maxHealth);
            staminabar.setMaxValue(maxStamina);
        }

        private int SetMaxHealthFromHealthLV()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        private int SetMaxStaminaFromHealthLV()
        {
            maxStamina = staminaLevel * 10;
            return maxStamina;
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
    }
}
