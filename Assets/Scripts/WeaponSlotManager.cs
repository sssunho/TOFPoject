using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TOF
{
    public class WeaponSlotManager : MonoBehaviour
    {
        PlayerManager playerManager;
        PlayerInventory playerInventory;

        public WeaponItem attackingWeapon;
        public WeaponHolderSlot leftHandSlot;
        public WeaponHolderSlot rightHandSlot;
        public WeaponHolderSlot backSlot;
        public WeaponHolderSlot HolderSlot;

        public DamageCollider leftHandDamageCollider;
        public DamageCollider rightHandDamageCollider;

        Animator animator;

        QuickSlotUI quickSlotUI;

        InputHandler inputHandler;

        PlayerStats playerStats;
        PlayerEffectManager playerEffectManager;
        PlayerAnimationManager playerAnimationManager;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            quickSlotUI = FindObjectOfType<QuickSlotUI>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = FindObjectOfType<PlayerInventory>();
            playerEffectManager = GetComponent<PlayerEffectManager>();
            playerAnimationManager = GetComponent<PlayerAnimationManager>();
        }

        public void LoadBotWeaponsOnSlot() 
        {
            LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            LoadWeaponOnSlot(playerInventory.leftWeapon, true);
        }

        public void LoadWeaponOnSlot(WeaponItem weaponItem, bool isLeft)
        {
            if (isLeft)
            {
                leftHandSlot.currentWeapon = weaponItem;
                leftHandSlot.LoadWeaponModel(weaponItem);
                LoadLeftWeaponDamageCollider();
                quickSlotUI.UpdateWeaponQuickSlot(true, weaponItem);
                #region Handle Weapon Idle Aniamations

                if (weaponItem != null)
                {
                    animator.CrossFade(weaponItem.left_hand_idle, 0.2f);
                }
                else
                {
                    animator.CrossFade("Left Arm Empty", 0.2f);
                }

                #endregion
            }
            else
            {
                if (inputHandler.interactFlag && inputHandler.e_Input)
                {
                    HolderSlot.LoadWeaponModel(rightHandSlot.currentWeapon);
                    backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                    rightHandSlot.UnloadWeaponAndDestory();
                    leftHandSlot.UnloadWeaponAndDestory();
                    animator.CrossFade(weaponItem.th_idle, 0.2f);
                }
                else
                {
                    if (inputHandler.twoHandFlag)
                    {
                        backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                        leftHandSlot.UnloadWeaponAndDestory();
                        animator.CrossFade(weaponItem.th_idle, 0.2f);
                    }
                    else
                    {
                        #region Handle Weapon Idle Aniamations

                        animator.CrossFade("Both Arm Empty", 0.1f);

                        backSlot.UnloadWeaponAndDestory();
                        HolderSlot.UnloadWeaponAndDestory();
                        if (weaponItem != null)
                        {
                            animator.CrossFade(weaponItem.right_hand_idle, 0.2f);
                        }
                        else
                        {
                            animator.CrossFade("Right Arm Empty", 0.2f);
                        }

                        #endregion
                    }
                    rightHandSlot.currentWeapon = weaponItem;
                    rightHandSlot.LoadWeaponModel(weaponItem);
                    LoadRightWeaponDamageCollider();
                    quickSlotUI.UpdateWeaponQuickSlot(false, weaponItem);
                }
            }
        }

        #region Handle Weapon's Damage Collider

        private void LoadLeftWeaponDamageCollider()
        {
            leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            leftHandDamageCollider.currentWeaponDamage = playerInventory.leftWeapon.baseDamage;
            leftHandDamageCollider.senderStat = GetComponentInParent<CharacterStats>();
            leftHandDamageCollider.teamIDNumber = playerStats.teamIDNumber;
            playerEffectManager.leftWeaponFX = leftHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        private void LoadRightWeaponDamageCollider()
        {
            rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
            rightHandDamageCollider.currentWeaponDamage = playerInventory.rightWeapon.baseDamage;
            rightHandDamageCollider.senderStat = GetComponentInParent<CharacterStats>();
            rightHandDamageCollider.teamIDNumber = playerStats.teamIDNumber;

            playerEffectManager.rightWeaponFX = rightHandSlot.currentWeaponModel.GetComponentInChildren<WeaponFX>();
        }

        public void OpenDamageCollider()
        {
            if (playerManager.isUsingRightHand)
            {
                rightHandDamageCollider.EnableDamageCollider();
            }
            else if(playerManager.isUsingLeftHand)
            {
                leftHandDamageCollider.EnableDamageCollider();
            }
        }
        public void CloseDamageCollider()
        {
            rightHandDamageCollider.DisableDamageCollider();
            leftHandDamageCollider.DisableDamageCollider();
        }

        #endregion

        #region Handle Weapon's Stamina Drain

        public void DrainStaminaLightAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.lightAttackMultiplier));
        }

        public void DrainStaminaHeavyAttack()
        {
            playerStats.TakeStaminaDamage(Mathf.RoundToInt(attackingWeapon.baseStamina * attackingWeapon.heavyAttackMultiplier));
        }

        #endregion
    }
}
