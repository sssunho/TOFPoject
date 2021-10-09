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

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLV();
            currentHealth = maxHealth;
            healthbar.setMaxValue(maxHealth);
        }

        private int SetMaxHealthFromHealthLV()
        {
            maxHealth = healthLevel * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            healthbar.setCurValue(currentHealth);
        }
    }
}
