using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class PlayerStats : CharacterStats
    {
        GameManager gameManager;
        HUDControl HUD;
        WeaponSlotManager weaponSlotManager;

        public SliderControl healthbar;
        public SliderControl staminabar;
        public SliderControl focusPointBar;

        public float staminaRegenerationAmount = 1;
        public float staminaRegenTimer;

        [Header("Primary Stats")]
        public int lv = 1;
        public int xp = 0;
        public int hp = 0;
        public int mp = 0;
        public int sp = 0;
        public int str = 0;
        public int dex = 0;

        public override int Atk
        {
            get
            {
                if (weaponSlotManager.rightHandSlot.currentWeapon == null) return str;
                return str + weaponSlotManager.rightHandSlot.currentWeapon.baseDamage;
            }
        }

        public override int Def
        {
            get
            {
                return str;
            }
        }

        public override float Mov
        {
            get
            {
                return 1.0f + (float)dex / 100.0f;
            }
        }
        public override float Crit
        {
            get
            {
                return (float)dex / 100.0f;
            }
        }

        private void Awake()
        {
            characterManager = GetComponent<PlayerManager>();
            animatorManager = GetComponentInChildren<PlayerAnimationManager>();
            effectManager = GetComponentInChildren<CharacterEffectManager>();
            gameManager = FindObjectOfType<GameManager>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            HUD = FindObjectOfType<HUDControl>();
            healthbar = HUD.healthSlider.GetComponent<SliderControl>();
            staminabar = HUD.staminaSlider.GetComponent<SliderControl>();
            focusPointBar = HUD.focusSlider.GetComponent<SliderControl>();

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

        public override void TakeDamage(Damage damage)
        {
            base.TakeDamage(damage);

            healthbar.setCurValue(currentHealth);

        }

        public void TakeDamage(int damage, string damageAnimation = "Damage_01")
        {
            if (characterManager.isInvulnerable) return;

            if (isDead) return;

            currentHealth -= damage;
            healthbar.setCurValue(currentHealth);

            animatorManager.PlayTargetAnimation(damageAnimation, true);

            if(currentHealth<=0)
            {
                currentHealth = 0;
                if (damageAnimation == "Fall Death")
                    animatorManager.PlayTargetAnimation("Fall Death", true);
                else
                    animatorManager.PlayTargetAnimation("Dead_01", true);
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
            if (characterManager.isInteracting)
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

        public void BonFireHealingPlayer()
        {
            currentHealth = maxHealth;
            currentStamina = maxStamina;
            currentFocusPoint = maxFocusPoints;
            healthbar.setCurValue(currentHealth);
            staminabar.setCurValue(currentStamina);
            focusPointBar.setCurValue(currentFocusPoint);           
        }
    }
}
