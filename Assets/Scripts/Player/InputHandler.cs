using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class InputHandler : MonoBehaviour
    {
        public float horizontal;
        public float vertical;
        public float moveAmount;
        public float mouseX;
        public float mouseY;

        public bool b_Input;
        public bool e_Input;
        public bool r_Input;
        public bool y_Input;
        public bool rb_Input;
        public bool rt_Input;
        public bool lt_Input;
        public bool lb_Input;
        public bool critical_Attack_Input;
        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

        public bool jump_Input;
        public bool inventory_Input;
        public bool lockOn_Input;
        public bool right_Stick_Right_Input;
        public bool right_Stick_Left_Input;

        public bool rollFlag;
        public bool twoHandFlag;
        public bool sprintFlag;
        public bool comboFlag;
        public bool inventoryFlag;
        public bool lockOnFlag;
        public float rollInputTimer;
        public bool interactFlag;

        public Transform criticalAttackRaycastStartPoint;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        EquipmentUI equipmentUI;
        BlockingCollider blockingCollider;
        PlayerManager playerManager;
        PlayerEffectManager playerEffectManager;
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;
        UIManager uiManager;
        CameraHandler cameraHandler;
        PlayerAnimationManager playerAnimationHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponentInChildren<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            playerEffectManager = GetComponentInChildren<PlayerEffectManager>();
            blockingCollider = GetComponentInChildren<BlockingCollider>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerAnimationHandler = GetComponentInChildren<PlayerAnimationManager>();
        }

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerControls();
                inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
                inputActions.PlayerMovement.Camera.performed += i => cameraInput = i.ReadValue<Vector2>();
                inputActions.PlayerActions.RB.performed += i => rb_Input = true;
                inputActions.PlayerActions.RT.performed += i => rt_Input = true;
                inputActions.PlayerActions.RT.canceled += i => rt_Input = false;
                inputActions.PlayerActions.LT.performed += i => lt_Input = true;
                inputActions.PlayerActions.LB.performed += i => lb_Input = true;
                inputActions.PlayerActions.LB.canceled += i => lb_Input = false;
                inputActions.PlayerActions.Roll.performed += i => b_Input = true;
                inputActions.PlayerActions.Roll.canceled += i => b_Input = false;
                inputActions.PlayerQuickSlot.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlot.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.Y.performed += inputActions => y_Input = true;
                inputActions.PlayerActions.R.performed += inputActions => r_Input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerActions.CriticaAttack.performed += i => critical_Attack_Input = true;
                inputActions.PlayerActions.CriticaAttack.canceled += i => critical_Attack_Input = false;
            }

            inputActions.Enable();
        }

        private void OnDisable()
        {
            inputActions.Disable();
        }

        public void TickInput(float delta)
        {
            MoveInput(delta);
            HandleRollInput(delta);
            HandleCombatInput(delta);
            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleInteractiongButtonInput();
            HandleTwoHandInput();
            HandleLockOnInput();
            HandleJumpInput();
            HandleCriticalAttackInput();
            HandleUseConsumableInput();
        }

        private void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            vertical = movementInput.y;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
            mouseX = cameraInput.x;
            mouseY = cameraInput.y;
        }

        private void HandleQuickSlotsInput()
        {
            inputActions.PlayerQuickSlot.DPadRight.performed += i => d_Pad_Right = true;
            inputActions.PlayerQuickSlot.DPadLeft.performed += i => d_Pad_Left = true;
            if (d_Pad_Right)
                playerInventory.changeRightWeapon();
            else if (d_Pad_Left)
                playerInventory.changeLeftWeapon();
        }

        private void HandleCombatInput(float delta)
        {
            if(rb_Input)
            {
                playerAttacker.HandleRBAction();
            }

            if (rt_Input)
            {
                if (critical_Attack_Input)
                    playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }

            if (!critical_Attack_Input && playerManager.isCharging)
                playerAttacker.ReleaseCharge();

            if(lt_Input)
            {
                if(twoHandFlag)
                {

                    //if two handing handle weapon
                }
                else
                {
                    playerAttacker.HandleLTAction();
                    //else handle light attack or melee weapon 
                    //hanlde weapon art if shield 
                }
            }

            if(lb_Input)
            {
                playerAttacker.HandleLBAction();
            }
            else
            {
                playerManager.isBlocking = false;

                if(blockingCollider.enabled)
                {
                    blockingCollider.DisableBlockingCollider();
                }
            }
        }

        private void HandleRollInput(float delta)
        {
            if (b_Input)
            {
                rollInputTimer += delta;
                if(playerStats.currentStamina <= 0)
                {
                    b_Input = false;
                    sprintFlag = false;
                }
                if(moveAmount > 0.5f && playerStats.currentStamina > 0)
                {
                    sprintFlag = true;
                    playerStats.staminaRegenTimer = 0;
                }
            }
            else
            {
                sprintFlag = false;
                if (rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    rollFlag = true;
                }
                rollInputTimer = 0;
            }
        }

        public void HandleInventoryInput()
        {
            inputActions.PlayerActions.Inventory.performed += i => inventory_Input = true;

            if(inventory_Input)
            {
                inventoryFlag = !inventoryFlag;
                if (inventoryFlag)
                {
                    uiManager.OpenSelectWindow();
                    uiManager.UpdateUI();
                    uiManager.hudWindow.SetActive(false);
                }
                else
                {
                    uiManager.CloseSelectWindow();
                    uiManager.CloseAllInventoryWindows();
                    uiManager.hudWindow.SetActive(true);
                }
            }
        }

        private void HandleInteractiongButtonInput()
        {
            inputActions.PlayerActions.E.performed += i => e_Input = true;
            if(interactFlag)
            {
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            }
            else
            {
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
            }
        }

        private void HandleTwoHandInput()
        {
            if(y_Input)
            {
                y_Input = false;
                twoHandFlag = !twoHandFlag;

                if(twoHandFlag)
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                }
                else
                {
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
                    weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
                }
            }
        }

        private void HandleLockOnInput()
        {
            if(lockOn_Input && lockOnFlag == false)
            {
                lockOn_Input = false;
                cameraHandler.HandleLokOn();
                if(cameraHandler.nearestLockOnTarget!=null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.nearestLockOnTarget;
                    lockOnFlag = true;
                    playerAnimationHandler.anim.SetFloat("isStrafing", 1);
                }
            }
            else if(lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
                playerAnimationHandler.anim.SetFloat("isStrafing", 0);
            }

            if (lockOnFlag && right_Stick_Left_Input)
            {
                right_Stick_Left_Input = false;
                cameraHandler.HandleLokOn();
                if (cameraHandler.leftLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.leftLockTarget;
                }
            }

            if (lockOnFlag && right_Stick_Right_Input)
            {
                right_Stick_Right_Input = false;
                cameraHandler.HandleLokOn();
                if (cameraHandler.rightLockTarget != null)
                {
                    cameraHandler.currentLockOnTarget = cameraHandler.rightLockTarget;
                }
            }

            cameraHandler.SetCameraHeight();
        }

        private void HandleCriticalAttackInput()
        {
            if (playerManager.isCharging) return;

            if (critical_Attack_Input)
            {
                critical_Attack_Input = false;
                playerAttacker.AttempBackStabOrRiposte();
            }
        }

        private void HandleJumpInput()
        {
            inputActions.PlayerActions.Jump.performed += i => jump_Input = true;
        }

        private void HandleUseConsumableInput()
        {
            if (r_Input)
            {
                r_Input = false;
                playerInventory.currentConsumable.AttempToConsumeItem(playerAnimationHandler, weaponSlotManager, playerEffectManager);
            }
        }
    }
}