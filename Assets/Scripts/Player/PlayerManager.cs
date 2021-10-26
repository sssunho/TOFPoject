using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PlayerManager : CharacterManager
    {
        InputHandler inputHandler;
        Animator anim;
        CameraHandler cameraHandler;
        PlayerLocomotion playerLocomotion;
        PlayerStats playerStats;
        PlayerAnimationManager playerAnimatorManager;
        GameManager gameManager;

        InteractableUI interactableUI;
        public GameObject interactableUIGameObject;
        public GameObject itemInteractableUIGameObject;

        [Header("Player Flags")]
        public bool isSprinting;
        public bool isInAir;
        public bool isGrounded;
        public bool canDoCombo;
        public bool isUsingRightHand;
        public bool isUsingLeftHand;
        public bool isCharging;
        public bool isBonFire;
        public bool isPassing;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            backStabCollider = GetComponentInChildren<CriticalDamageCollider>();
            playerStats = GetComponent<PlayerStats>();
            playerAnimatorManager = GetComponentInChildren<PlayerAnimationManager>();
            gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            inputHandler = GetComponent<InputHandler>();
            playerLocomotion = GetComponent<PlayerLocomotion>();
            anim = GetComponentInChildren<Animator>();
            interactableUI = FindObjectOfType<InteractableUI>();
            interactableUIGameObject = interactableUI.interaction_Popup;
            itemInteractableUIGameObject = interactableUI.interaction_Item;
        }

        private void Update()
        {
            float delta = Time.deltaTime;

            isInteracting = anim.GetBool("isInteracting");
            canDoCombo = anim.GetBool("canDoCombo");
            anim.SetBool("isBlocking", isBlocking);
            isUsingRightHand = anim.GetBool("isUsingRightHand");
            isUsingLeftHand = anim.GetBool("isUsingLeftHand");
            isInvulnerable = anim.GetBool("isInvulnerable");
            isCharging = anim.GetBool("isCharging");
            anim.SetBool("isInAir", isInAir);
            anim.SetBool("isDead", playerStats.isDead);
            anim.SetBool("isBonFire", isBonFire);

            inputHandler.TickInput(delta);
            playerAnimatorManager.canRotate = anim.GetBool("canRotate");
            playerLocomotion.HandleRollingAndSprinting(delta);
            playerLocomotion.HandleJump();
            playerStats.RegenerateStamina();

            playerLocomotion.HandleMovement(delta);

            CheckForInteractableObject();
        }

        private void FixedUpdate()
        {
            float delta = Time.fixedDeltaTime;
            playerLocomotion.HandleFalling(delta, playerLocomotion.moveDirection);
            //playerLocomotion.HandleMovement(delta);
            playerLocomotion.HandleRotation(delta);
        }

        private void LateUpdate()
        {
            inputHandler.rollFlag = false;
            inputHandler.rb_Input = false;
            //inputHandler.rt_Input = false;
            inputHandler.lt_Input = false;
            inputHandler.d_Pad_Up = false;
            inputHandler.d_Pad_Down = false;
            inputHandler.d_Pad_Right = false;
            inputHandler.d_Pad_Left = false;
            inputHandler.inventory_Input = false;
            inputHandler.e_Input = false;
            inputHandler.jump_Input = false;

            float delta = Time.deltaTime;

            if (cameraHandler != null && !isBonFire && !playerStats.isDead)
            {
                cameraHandler.FollowTarget(delta);
                cameraHandler.HandleCameraRotation(delta, inputHandler.mouseX, inputHandler.mouseY);
            }

            if (isInAir)
            {
                playerLocomotion.inAirTimer += Time.deltaTime;
            }
        }

        #region Player Interactions

        public void CheckForInteractableObject()
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f, cameraHandler.ignoreLayer))
            {
                if(hit.collider.tag == "Interactable")
                {
                    Interactable interactableObject = hit.collider.GetComponent<Interactable>();

                    if (interactableObject != null)
                    {
                        string interactableText = interactableObject.interactableText;
                        interactableUI.interactableText.text = interactableText;
                        interactableUIGameObject.SetActive(true);
                        inputHandler.interactFlag = true;
                        if (inputHandler.e_Input)
                            hit.collider.GetComponent<Interactable>().Interact(this);
                    }
                }
            }
            else
            {
                if(interactableUIGameObject != null)
                {
                    interactableUIGameObject.SetActive(false);
                }

                if(itemInteractableUIGameObject !=null && inputHandler.e_Input)
                {
                    itemInteractableUIGameObject.SetActive(false);
                }
                inputHandler.interactFlag = false;
            }
        }

        public void OpenChestInteraction(Transform playerStandsHereWhenOpeningChest)
        {
            //playerLocomotion.rigidbody.velocity = Vector3.zero; // Strops the player from ice staking
            playerLocomotion.controller.Move(Vector3.zero);
            transform.position = playerStandsHereWhenOpeningChest.transform.position;
            playerAnimatorManager.PlayTargetAnimation("Open Chest", true);
        }

        public void PassThroughFogWallInteraction(Transform fogWallEnterance)
        {
            //playerLocomotion.rigidbody.velocity = Vector3.zero; // Strops the player from ice staking
            playerLocomotion.controller.Move(Vector3.zero);

            Vector3 rotationDirection = fogWallEnterance.transform.forward;
            Quaternion turnRotation = Quaternion.LookRotation(rotationDirection);
            transform.rotation = turnRotation;

            playerAnimatorManager.PlayTargetAnimation("Pass Through Fog", true);
        }

        public void BonFireInteraction(Bonfire bonfire)
        {
            playerLocomotion.controller.Move(Vector3.zero);
            playerStats.BonFireHealingPlayer();
            gameManager.SetSpawnSpoint(bonfire.checkPoint);
            if (bonfire.isIgnited)
            {
                playerAnimatorManager.PlayTargetAnimation("Bonfire Start", true);
                isBonFire = true;
                // UI 포탈 선택창 만들기.
                gameManager.TeleportWindow.SetActive(true);
                gameManager.playerUI.SetActive(false);
            }
            else
            {
                playerAnimatorManager.PlayTargetAnimation("Bonfire Ignite", true);
                gameManager.UpdateCheckPoint(bonfire.name);
            }
        }

        #endregion
    }
}