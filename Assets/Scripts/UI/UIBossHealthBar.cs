using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class UIBossHealthBar : MonoBehaviour
    {
        public Text bossName;
        public Slider slider;

        private void Start()
        {
            SetHealthBarInactive();
        }

        public void SetBossName(string name)
        {
            bossName.text = name;
        }

        public void SetUIHealthBarToActive()
        {
            slider.gameObject.SetActive(true);
            bossName.gameObject.SetActive(true);
        }

        public void SetHealthBarInactive()
        {
            slider.gameObject.SetActive(false);
            bossName.gameObject.SetActive(false);
        }

        public void SetBossMaxHealth(int maxHealth)
        {
            slider.maxValue = maxHealth;
            slider.value = maxHealth;
        }

        public void SetBossCurrentHealth(int currentHealth)
        {
            slider.value = currentHealth;
        }
    }
}