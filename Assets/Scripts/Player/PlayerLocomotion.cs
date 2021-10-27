using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PlayerLocomotion : MonoBehaviour
    {
        CameraHandler cameraHandler;
        PlayerManager playerManager;
        PlayerStats playerStats;
        Transform cameraObject;
        InputHandler inputHandler;
        public Vector3 moveDirection;

        [HideInInspector]
        public bool ignoreGravity = false;

       [HideInInspector]
        public Transform myTransform;
        [HideInInspector]
        public PlayerAnimationManager playerAnimationManager;

        public new Rigidbody rigidbody;
        public CharacterController controller;
        public GameObject normalCamera;

        [Header("Ground & Air Detection")]
        LayerMask ignoreForGroundCheck;
        public float inAirTimer;
        public float fallMinHeight = 6f;
        public float heightReached;
        public float fallDamage = 10;
        public bool isJump = false;

        [Header("Movement Stats")]
        [SerializeField]
        float movementSpeed = 5;
        [SerializeField]
        float walkingSpeed = 1;
        [SerializeField]
        float sprintSpeed = 7;
        [SerializeField]
        float rotationSpeed = 10;

        [Header("Stamina Cost")]
        [SerializeField]
        int rollStaminaCost = 15;
        [SerializeField]
        int backStepStaminaCost = 12;
        [SerializeField]
        int sprintStaminaCost = 1;
        [SerializeField]
        int jumpStaminaCost = 10;

        //public CapsuleCollider characterCollider;
        public CapsuleCollider characterCollisionBlockerCollider;

        private void Awake()
        {
            cameraHandler = FindObjectOfType<CameraHandler>();
            playerManager = GetComponent<PlayerManager>();
            playerStats = GetComponent<PlayerStats>();
            rigidbody = GetComponent<Rigidbody>();
            inputHandler = GetComponent<InputHandler>();
            playerAnimationManager = GetComponentInChildren<PlayerAnimationManager>();
            controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            cameraObject = Camera.main.transform;
            myTransform = this.transform;
            playerAnimationManager.Initialize();
            playerManager.isGrounded = true;
            ignoreForGroundCheck = ~(1 << 8 | 1 << 9 | 1 << 11);
            //Physics.IgnoreCollision(characterCollider, characterCollisionBlockerCollider, true);
        }

        #region Movement
        Vector3 normalVector;
        Vector3 targetPosition;

        public void HandleRotation(float delta)
        {
            if (playerAnimationManager.canRotate)
            {
                if (inputHandler.lockOnFlag)
                {
                    if (inputHandler.sprintFlag || inputHandler.rollFlag)
                    {
                        Vector3 targetDirection = Vector3.zero;
                        targetDirection = cameraHandler.cameraTransform.forward * inputHandler.vertical;
                        targetDirection += cameraHandler.cameraTransform.right * inputHandler.horizontal;
                        targetDirection.Normalize();
                        targetDirection.y = 0;

                        if (targetDirection == Vector3.zero)
                        {
                            targetDirection = transform.forward;
                        }

                        Quaternion tr = Quaternion.LookRotation(targetDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);

                        transform.rotation = targetRotation;
                    }
                    else
                    {
                        Vector3 rotationDirection = moveDirection;
                        rotationDirection = cameraHandler.currentLockOnTarget.transform.position - transform.position;
                        rotationDirection.y = 0;
                        rotationDirection.Normalize();

                        Quaternion tr = Quaternion.LookRotation(rotationDirection);
                        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * Time.deltaTime);
                        transform.rotation = targetRotation;
                    }
                }
                else
                {
                    Vector3 targetDir = Vector3.zero;
                    float moveOverride = inputHandler.moveAmount;

                    targetDir = cameraObject.forward * inputHandler.vertical;
                    targetDir += cameraObject.right * inputHandler.horizontal;

                    targetDir.Normalize();
                    targetDir.y = 0;

                    if (targetDir == Vector3.zero)
                        targetDir = myTransform.forward;

                    float rs = rotationSpeed;

                    Quaternion tr = Quaternion.LookRotation(targetDir);
                    Quaternion targetRotation = Quaternion.Slerp(myTransform.rotation, tr, rs * delta);

                    myTransform.rotation = targetRotation;
                }
            }
        }

        public void HandleMovement(float delta)
        {
            if (inputHandler.rollFlag)
                return;

            if (playerManager.isInteracting)
                return;

            moveDirection = cameraObject.forward * inputHandler.vertical;
            moveDirection += cameraObject.right * inputHandler.horizontal;
            moveDirection.Normalize();
            moveDirection.y = 0;

            float speed = movementSpeed * playerStats.Mov;

            if (inputHandler.sprintFlag && inputHandler.moveAmount > 0.5)
            {
                speed = sprintSpeed * playerStats.Mov;
                playerManager.isSprinting = true;
                moveDirection *= speed;
                // #. sprint stamina 1/10 version.
                //if(sprintStaminaCost == 10)
                //{
                //    sprintStaminaCost = 1;
                //    playerStats.TakeStaminaDamage(sprintStaminaCost);
                //}
                //sprintStaminaCost++;
                playerStats.TakeStaminaDamage(sprintStaminaCost);
            }
            else
            {
                if (inputHandler.moveAmount < 0.5f)
                {
                    moveDirection *= walkingSpeed * playerStats.Mov;
                    playerManager.isSprinting = false;
                }
                else
                {
                    moveDirection *= speed;
                    playerManager.isSprinting = false;
                }
            }

            Vector3 projectedVelocity = Vector3.ProjectOnPlane(moveDirection, normalVector);
            //rigidbody.velocity = projectedVelocity;
            projectedVelocity.y -= 9.81f;
            controller.Move(projectedVelocity * delta);

            if (inputHandler.lockOnFlag && inputHandler.sprintFlag==false)
            {
                playerAnimationManager.UpdateAnimatorValues(inputHandler.vertical, inputHandler.horizontal, playerManager.isSprinting);
            }
            else
            {
                playerAnimationManager.UpdateAnimatorValues(inputHandler.moveAmount, 0, playerManager.isSprinting);
            }
        }

        public void HandleRollingAndSprinting(float delta)
        {
            if (playerAnimationManager.anim.GetBool("isInteracting"))
                return;

            // Check if we have stamina
            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.rollFlag)
            {
                moveDirection = cameraObject.forward * inputHandler.vertical;
                moveDirection += cameraObject.right * inputHandler.horizontal;

                if (inputHandler.moveAmount > 0)
                {
                    playerAnimationManager.PlayTargetAnimation("Rolling", true);
                    moveDirection.y = 0;
                    Quaternion rollRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = rollRotation;
                    playerStats.TakeStaminaDamage(rollStaminaCost);
                }
                else
                {
                    playerAnimationManager.PlayTargetAnimation("Backstep", true);
                    playerStats.TakeStaminaDamage(backStepStaminaCost);
                }
            }
        }

        public void HandleFalling(float delta, Vector3 moveDirection)
        {
            if (controller.isGrounded)
            {
                HandleFallDamage();
                inAirTimer = 0;
                if(!playerManager.isGrounded)
                {
                    playerAnimationManager.PlayTargetAnimation("Land", true);
                    playerManager.isGrounded = true;
                    heightReached = transform.position.y;
                }
                return;
            }
            if(!isJump)
                inAirTimer += Time.deltaTime;
            if(inAirTimer > 0.2f && playerManager.isGrounded)
            {
                playerAnimationManager.PlayTargetAnimation("Falling", true);
                playerManager.isGrounded = false;
            }
        }

        public void HandleFallDamage()
        {
            float fallHeight = (heightReached - transform.position.y);
            fallHeight -= fallMinHeight;
            if(fallHeight > 0)
            {
                int damage = (int)(fallDamage * fallHeight);
                playerStats.TakeDamage(damage, "Fall");
                playerManager.isGrounded = true;
                heightReached = transform.position.y;
            }
        }

        public void HandleJump()
        {
            if (playerManager.isInteracting)
                return;

            if (playerStats.currentStamina <= 0)
                return;

            if (inputHandler.jump_Input)
            {
                if (inputHandler.moveAmount > 0)
                {
                    moveDirection = cameraObject.forward * inputHandler.vertical;
                    moveDirection += cameraObject.right * inputHandler.horizontal;
                    playerAnimationManager.PlayTargetAnimation("Jump", true);
                    moveDirection.y = 0;
                    Quaternion jumpRotation = Quaternion.LookRotation(moveDirection);
                    myTransform.rotation = jumpRotation;
                    isJump = true;
                    playerStats.TakeStaminaDamage(jumpStaminaCost);
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "DeadZone")
            {
                playerStats.TakeDamage(playerStats.maxHealth, "Fall Death");
                cameraHandler.cameraPivotTransform.Rotate(45, 0, 0);
            }
        }
        #endregion
    }
}
