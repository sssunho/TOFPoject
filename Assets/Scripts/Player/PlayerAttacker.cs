using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PlayerAttacker : MonoBehaviour
    {
        PlayerAnimationManager playerAnimationHandler;
        PlayerEquipmentManager playerEquipmentManager;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        PlayerManager playerManager;
        PlayerStats playerStats;
        PlayerInventory playerInventory;
        PlayerEffectManager playerEffectManager;

        public string lastAttack;

        LayerMask backStabLayer = 1 << 12;
        LayerMask riposteLayer = 1 << 13;

        private void Awake()
        {
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerAnimationHandler = GetComponent<PlayerAnimationManager>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = FindObjectOfType<PlayerInventory>();
            playerEffectManager = GetComponent<PlayerEffectManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                playerAnimationHandler.anim.SetBool("canDoCombo", false);
                playerManager.isBlocking = false;

                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    playerAnimationHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }
                else if (lastAttack == weapon.TH_Light_Attack_1)
                {
                    playerAnimationHandler.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            playerAnimationHandler.anim.SetBool("isUsingRightHand", true);
            playerManager.isBlocking = false;
            weaponSlotManager.attackingWeapon = weapon;
            playerAnimationHandler.anim.SetBool("isHeavyAttack", false);
            playerAnimationHandler.anim.SetInteger("weaponID", inputHandler.twoHandFlag ? weapon.TwohandedWeaponID : weapon.OnehandedWeaponID);
            playerAnimationHandler.SetInteraction(true);

            //if (inputHandler.twoHandFlag)
            //{
            //    playerAnimationHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
            //    lastAttack = weapon.TH_Light_Attack_1;
            //}
            //else
            //{
            //    playerAnimationHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
            //    lastAttack = weapon.OH_Light_Attack_1;
            //}
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            if (playerManager.isInteracting) return;

            if (playerStats.currentStamina <= 0)
                return;

            playerAnimationHandler.anim.SetBool("isUsingRightHand", true);
            playerAnimationHandler.anim.SetTrigger("AttackTrigger");
            playerAnimationHandler.anim.SetBool("isHeavyAttack", true);
            playerAnimationHandler.anim.SetInteger("weaponID", inputHandler.twoHandFlag ? weapon.TwohandedWeaponID : weapon.OnehandedWeaponID);
            playerAnimationHandler.SetInteraction(true);
            playerManager.isUsingLeftHand = true;
            playerManager.isBlocking = false;
            playerManager.isCharging = true;
            //weaponSlotManager.attackingWeapon = weapon;
            //if (inputHandler.twoHandFlag)
            //{

            //}
            //else
            //{

            //}
            //playerAnimationHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            //lastAttack = weapon.OH_Heavy_Attack_1;
        }

        #region Input Action
        public void HandleRBAction()
        {
            if (playerInventory.rightWeapon.isMeleeWeapon)
            {
                // Handle Melee Action
                PerformRBMeleeAction();
            }
            else if (playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
            {
                // Handle Magic Action
                PerformRBMagicAction(playerInventory.rightWeapon);
                // Handle Miracle Action

                // Handle Pyro Action

            }
        }

        public void HandleLTAction()
        {
            if (playerInventory.leftWeapon.isShieldWeapon)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
                //perform shield weapon;
            }
            else if (playerInventory.leftWeapon.isMeleeWeapon)
            {
                //do light attack
            }
        }

        public void HandleLBAction()
        {
            PerformLBBlcokingAction();
        }

        public void ReleaseCharge()
        {
            playerAnimationHandler.anim.SetBool("isCharging", false);
        }

        #endregion

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            playerAnimationHandler.anim.SetTrigger("AttackTrigger");
            HandleLightAttack(playerInventory.rightWeapon);

            //if (playerManager.canDoCombo)
            //{
            //    inputHandler.comboFlag = true;
            //    HandleWeaponCombo(playerInventory.rightWeapon);
            //    inputHandler.comboFlag = false;
            //}
            //else if (!playerManager.canDoCombo && !playerManager.isInteracting)
            //{
            //    playerAnimationHandler.anim.SetBool("isUsingRightHand", true);
            //    HandleLightAttack(playerInventory.rightWeapon);
            //}
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if (weapon.isFaithCaster)
            {
                if (playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell && playerStats.currentFocusPoint > 0)
                {
                    playerInventory.currentSpell.AttemptToCastSpell(playerAnimationHandler, playerStats);
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting) return;

            playerManager.isBlocking = false;

            if (isTwoHanding)
            {
                //if we are two handing preform weapon art for right weapon

            }
            else
            {
                playerManager.isInteracting = true;
                playerAnimationHandler.PlayTargetAnimation(playerInventory.leftWeapon.weapon_art, true);
            }

        }

        private void SuccessfullyCastSpell()
        {
            playerInventory.currentSpell.SuccessfullyCastSpell(playerAnimationHandler, playerStats);
        }

        #endregion

        #region Defense Actions

        private void PerformLBBlcokingAction()
        {
            if (playerManager.isInteracting) return;

            if (playerManager.isBlocking) return;

            if (playerAnimationHandler.anim.GetFloat("isStrafing") == 0)
                playerAnimationHandler.anim.SetFloat("isStrafing", 2);
            playerAnimationHandler.PlayTargetAnimation("Block Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        #endregion

        #region Backstab or Riposte Behaviors
        public void AttempBackStabOrRiposte()
        {
            if (playerStats.currentStamina <= 0)
                return;

            RaycastHit hit;
            if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, backStabLayer))
            {
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null)
                {
                    //Check for team id (so you cant back stab friend or yourself)
                    playerManager.transform.position = enemyCharacterManager.backStabCollider.criticalDamageStandPosition.position;
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimationHandler.PlayTargetAnimation("BackStab", true);
                    enemyCharacterManager.GetComponentInChildren<EnemyAnimationManager>().PlayTargetAnimation("BackStabbed", true);
                    //do damage
                }
            }
            else if (Physics.Raycast(inputHandler.criticalAttackRaycastStartPoint.position,
                transform.TransformDirection(Vector3.forward), out hit, 0.5f, riposteLayer))
            {
                //Check for team id (so you cant back stab friend or yourself)
                CharacterManager enemyCharacterManager = hit.transform.gameObject.GetComponentInParent<CharacterManager>();
                DamageCollider rightWeapon = weaponSlotManager.rightHandDamageCollider;

                if (enemyCharacterManager != null && enemyCharacterManager.canBeRiposted)
                {
                    playerManager.transform.position = enemyCharacterManager.riposteCollider.criticalDamageStandPosition.position;
                    Vector3 rotationDirection = playerManager.transform.root.eulerAngles;
                    rotationDirection = hit.transform.position - playerManager.transform.position;
                    rotationDirection.y = 0;
                    rotationDirection.Normalize();
                    Quaternion tr = Quaternion.LookRotation(rotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 500 * Time.deltaTime);
                    playerManager.transform.rotation = targetRotation;

                    int criticalDamage = playerInventory.rightWeapon.criticalDamageMultiplier * rightWeapon.currentWeaponDamage;
                    enemyCharacterManager.pendingCriticalDamage = criticalDamage;

                    playerAnimationHandler.PlayTargetAnimation("Riposte", true);
                    enemyCharacterManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Riposted", true);
                }
            }
        }

        private bool IsSeeEachOther(in Transform v1, in Transform v2, float limitAngle = 30.0f)
        {
            Vector3 rel = v2.position - v1.position;
            return Vector3.Angle(rel, v1.forward) < limitAngle && Vector3.Angle(rel, -v2.forward) < limitAngle && Vector3.Angle(rel, -v2.forward) < limitAngle;
        }

        private void ParryBehavior()
        {
            int maxColliders = 10;
            float parryRange = 0.8f;

            Collider[] hitColliders = new Collider[maxColliders];
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, parryRange, hitColliders, 1 << 9);
            for (int i = 0; i < numColliders; i++)
            {
                if (hitColliders[i].tag == "Enemy")
                {
                    EnemyManager enemyManager = hitColliders[i].gameObject.GetComponent<EnemyManager>();
                    if (enemyManager != null)
                    {
                        if (IsSeeEachOther(transform, hitColliders[i].gameObject.transform) && enemyManager.canBeParried)
                        {
                            enemyManager.canBeParried = false;
                            playerManager.isParrying = false;
                            enemyManager.GetComponentInChildren<AnimatorManager>().PlayTargetAnimation("Parried", true);
                            enemyManager.GetComponentInChildren<EnemyWeaponSlotManager>().CloseDamageCollider();
                            break;
                        }
                    }
                }
            }
        }
        #endregion

        public void SetRightWeaponEffect()
        {
            playerEffectManager.PlayWeaponFX(false);
        }

        public void SetLeftWeaponEffect()
        {
            playerEffectManager.PlayWeaponFX(true);
        }

        private void FixedUpdate()
        {
            if (playerManager.isParrying)
            {
                ParryBehavior();
            }
        }
    }
}