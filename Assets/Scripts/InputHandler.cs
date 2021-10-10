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
        public bool y_input;
        public bool rb_Input;
        public bool rt_Input;
        public bool critical_Attack_Input;
        public bool d_Pad_Up;
        public bool d_Pad_Down;
        public bool d_Pad_Left;
        public bool d_Pad_Right;

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

        public Transform criticalAttackRaycastStartPoint;

        PlayerControls inputActions;
        PlayerAttacker playerAttacker;
        PlayerInventory playerInventory;
        EquipmentUI equipmentUI;
        PlayerManager playerManager;
        WeaponSlotManager weaponSlotManager;
        UIManager uiManager;
        CameraHandler cameraHandler;

        Vector2 movementInput;
        Vector2 cameraInput;

        private void Awake()
        {
            playerAttacker = GetComponent<PlayerAttacker>();
            playerInventory = GetComponent<PlayerInventory>();
            playerManager = GetComponent<PlayerManager>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
            cameraHandler = FindObjectOfType<CameraHandler>();
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
                inputActions.PlayerQuickSlot.DPadRight.performed += i => d_Pad_Right = true;
                inputActions.PlayerQuickSlot.DPadLeft.performed += i => d_Pad_Left = true;
                inputActions.PlayerActions.Y.performed += inputActions => y_input = true;
                inputActions.PlayerActions.LockOn.performed += i => lockOn_Input = true;
                inputActions.PlayerMovement.LockOnTargetLeft.performed += i => right_Stick_Left_Input = true;
                inputActions.PlayerMovement.LockOnTargetRight.performed += i => right_Stick_Right_Input = true;
                inputActions.PlayerActions.CriticaAttack.performed += i => critical_Attack_Input = true;
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
            HandleAttackInput(delta);
            HandleQuickSlotsInput();
            HandleInventoryInput();
            HandleInteractiongButtonInput();
            HandleTwoHandInput();
            HandleLockOnInput();
            HandleCriticalAttackInput();
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

        private void HandleAttackInput(float delta)
        {

            if(rb_Input)
            {
                if (playerManager.canDoCombo)
                {
                    comboFlag = true;
                    playerAttacker.HandleWeaponCombo(playerInventory.rightWeapon);
                    comboFlag = false;
                }
                else if (!playerManager.canDoCombo && !playerManager.isInteracting)
                {
                    playerAttacker.HandleLightAttack(playerInventory.rightWeapon);
                }
            }
            if (rt_Input)
            {
                playerAttacker.HandleHeavyAttack(playerInventory.rightWeapon);
            }
        }

        private void HandleRollInput(float delta)
        {
            b_Input = inputActions.PlayerActions.Roll.phase == UnityEngine.InputSystem.InputActionPhase.Started;
            sprintFlag = b_Input;

            if (b_Input)
            {
                rollInputTimer += delta;
                //sprintFlag = true;
            }
            else
            {
                if(rollInputTimer > 0 && rollInputTimer < 0.5f)
                {
                    sprintFlag = false;
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
        }

        private void HandleTwoHandInput()
        {
            if(y_input)
            {
                y_input = false;
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
                }
            }
            else if(lockOn_Input && lockOnFlag)
            {
                lockOn_Input = false;
                lockOnFlag = false;
                cameraHandler.ClearLockOnTargets();
            }

            if(lockOnFlag && right_Stick_Left_Input)
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
            if(critical_Attack_Input)
            {
                critical_Attack_Input = false;
                playerAttacker.AttempBackStabOrRiposte();
            }
        }
    }
}