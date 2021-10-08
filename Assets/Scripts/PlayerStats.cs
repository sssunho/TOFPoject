using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class PlayerStats : MonoBehaviour
    {
        public int healthLV = 30;
        public int maxHealth;
        public int curHealth;

        public SliderControl healthbar;
        public SliderControl staminabar;
        public SliderControl strengthbar;

        private void Start()
        {
            maxHealth = SetMaxHealthFromHealthLV();
            curHealth = maxHealth;
            healthbar.setMaxValue(maxHealth);
        }

        private int SetMaxHealthFromHealthLV()
        {
            maxHealth = healthLV * 10;
            return maxHealth;
        }

        public void TakeDamage(int damage)
        {
            curHealth -= damage;
            healthbar.setCurValue(curHealth);
            Debug.Log(curHealth);
        }
    }
}
