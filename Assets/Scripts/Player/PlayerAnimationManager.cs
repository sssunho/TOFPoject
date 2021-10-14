using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PlayerAnimationManager : AnimatorManager
    {
        PlayerManager playerManager;
        PlayerStats playerStats;
        InputHandler inputHandler;
        PlayerLocomotion playerLocomotion;

        int vertical;
        int horizontal;
        int temp = 1;

        public void Initialize()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerLocomotion = GetComponentInParent<PlayerLocomotion>();
            anim = GetComponent<Animator>();
            playerStats = GetComponentInParent<PlayerStats>();
            vertical = Animator.StringToHash("Vertical");
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float verticalMovement, float horizontalMovement, bool isSprinting)
        {
            #region Vertical
            float v = 0;

            if (verticalMovement > 0 && verticalMovement < 0.55f)
            {
                v = 0.5f;
            }
            else if (verticalMovement > 0.55f)
            {
                v = 1;
            }
            else if (verticalMovement < 0 && verticalMovement > 0.55f)
            {
                v = -0.5f;
            }
            else if (verticalMovement < -0.55f)
            {
                v = -1;
            }
            else
            {
                v = 0;
            }
            #endregion

            #region Horizontal
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }
            #endregion  

            if (isSprinting)
            {
                v = 2;
                h = horizontalMovement;
            }

            anim.SetFloat(vertical, v, 0.1f, Time.deltaTime);
            anim.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }

        public void CanRotate()
        {
            anim.SetBool("canRotate", true);
        }

        public void StopRotation()
        {
            anim.SetBool("canRotate", false);
        }

        public void EnableCombo()
        {
            anim.SetBool("canDoCombo", true);
        }

        public void DisableCombo()
        {
            anim.SetBool("canDoCombo", false);
        }

        public void EnableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", true);
        }
        public void DisableIsInvulnerable()
        {
            anim.SetBool("isInvulnerable", false);
        }

        public void EnableCanBeRiposted()
        {
            playerManager.canBeRiposted = true;
        }

        public void DisableCanBeRiposted()
        {
            playerManager.canBeRiposted = false;
        }

        public void EnableIsParrying()
        {
            playerManager.isParrying = true;
        }

        public void DisableIsParrying()
        {
            playerManager.isParrying = false;
        }

        public void EnableCanBeParried()
        {
            playerManager.canBeParried = true;
        }

        public void DisableCanBeParried()
        {
            playerManager.canBeParried = false;
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            playerStats.TakeDamageNoAnimation(playerManager.pendingCriticalDamage);
            playerManager.pendingCriticalDamage = 0;
        }

        public void DisableCollision()
        {
            playerLocomotion.controller.enabled = false;
            playerLocomotion.characterCollisionBlockerCollider.enabled = false;
        }

        public void EnableCollision()
        {
            playerLocomotion.controller.enabled = true;
            playerLocomotion.characterCollisionBlockerCollider.enabled = true;
        }

        public void EndJump()
        {
            playerLocomotion.isJump = false;
            temp = 0;
        }

        private void OnAnimatorMove()
        {
            if (playerManager.isInteracting == false)
                return;

            float delta = Time.deltaTime;
            Vector3 deltaPosition = anim.deltaPosition;
            if (playerLocomotion.isJump)
            {
                if(temp < 75)
                {
                    deltaPosition.y += 3 * Time.deltaTime;
                    temp++;
                    Debug.Log(temp);
                }
                else
                {
                    deltaPosition.y -= 3f * Time.deltaTime;
                }
            }
            else
            {
                deltaPosition.y -= 9.81f * Time.deltaTime;
            }
            playerLocomotion.controller.Move(deltaPosition);
        }
    }
}