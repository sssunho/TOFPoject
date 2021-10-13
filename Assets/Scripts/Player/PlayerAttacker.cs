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
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerEffectManager = GetComponent<PlayerEffectManager>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.comboFlag)
            {
                playerAnimationHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    playerAnimationHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }
                else if(lastAttack == weapon.TH_Light_Attack_1)
                {
                    playerAnimationHandler.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            // ep 29에서 바뀐 부분입니다.
            // weaponSlotManager.attackingWeapon = weapon; 를 추가할 때
            // 이 주석을 지우고 추가하면 됩니다.
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;
            if(inputHandler.twoHandFlag)
            {
                playerAnimationHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
                lastAttack = weapon.TH_Light_Attack_1;
            }
            else
            {
                playerAnimationHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                lastAttack = weapon.OH_Light_Attack_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            // ep 29에서 바뀐 부분입니다.
            // weaponSlotManager.attackingWeapon = weapon; 를 추가할 때
            // 이 주석을 지우고 추가하면 됩니다.
            if (playerStats.currentStamina <= 0)
                return;

            weaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {

            }
            else
            {

            }
            playerAnimationHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            lastAttack = weapon.OH_Heavy_Attack_1;
        }

        #region Input Action
        public void HandleRBAction()
        {
            if(playerInventory.rightWeapon.isMeleeWeapon)
            {
                // Handle Melee Action
                PerformRBMeleeAction();
            }
            else if(playerInventory.rightWeapon.isSpellCaster || playerInventory.rightWeapon.isFaithCaster || playerInventory.rightWeapon.isPyroCaster)
            {
                // Handle Magic Action
                PerformRBMagicAction(playerInventory.rightWeapon);
                // Handle Miracle Action

                // Handle Pyro Action

            }
        }
        
        public void HandleLTAction()
        {
            if(playerInventory.leftWeapon.isShieldWeapon)
            {
                PerformLTWeaponArt(inputHandler.twoHandFlag);
                //perform shield weapon;
            }
            else if(playerInventory.leftWeapon.isMeleeWeapon)
            {
                //do light attack
            }
        }

        public void HandleLBAction()
        {
            PerformLBBlcokingAction();
        }
        #endregion

        #region Attack Actions
        private void PerformRBMeleeAction()
        {
            if (playerManager.canDoCombo)
            {
                inputHandler.comboFlag = true;
                HandleWeaponCombo(playerInventory.rightWeapon);
                inputHandler.comboFlag = false;
            }
            else if (!playerManager.canDoCombo && !playerManager.isInteracting)
            {
                playerAnimationHandler.anim.SetBool("isUsingRightHand", true);
                HandleLightAttack(playerInventory.rightWeapon);
            }

            playerEffectManager.PlayWeaponFX(false);
        }

        private void PerformRBMagicAction(WeaponItem weapon)
        {
            if(weapon.isFaithCaster)
            {
                if(playerInventory.currentSpell != null && playerInventory.currentSpell.isFaithSpell)
                {
                    playerInventory.currentSpell.AttemptToCastSpell(playerAnimationHandler, playerStats);
                }
            }
        }

        private void PerformLTWeaponArt(bool isTwoHanding)
        {
            if (playerManager.isInteracting) return;

            if(isTwoHanding)
            {
                //if we are two handing preform weapon art for right weapon

            }
            else
            {
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

            playerAnimationHandler.PlayTargetAnimation("Block Start", false, true);
            playerEquipmentManager.OpenBlockingCollider();
            playerManager.isBlocking = true;
        }

        #endregion

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

                if(enemyCharacterManager != null)
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

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            if(inputHandler)
                Gizmos.DrawSphere(inputHandler.criticalAttackRaycastStartPoint.position, 0.8f);
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

        private void FixedUpdate()
        {
            if (playerManager.isParrying)
            {
                ParryBehavior();
            }
        }
    }
}
