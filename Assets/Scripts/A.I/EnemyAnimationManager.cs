using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TOF
{
    public class EnemyAnimationManager : AnimatorManager
    {
        EnemyManager enemyManager;
        EnemyStats enemyStats;
        EnemyLocomotionManager enemyLocomotion;
        EnemyEffectManager enemyEffectManager;
        public NavMeshAgent agent;

        protected virtual void Awake()
        {
            anim = GetComponent<Animator>();
            enemyManager = GetComponentInParent<EnemyManager>();
            enemyStats = GetComponentInParent<EnemyStats>();
            enemyEffectManager = GetComponent<EnemyEffectManager>();
            enemyLocomotion = GetComponentInParent<EnemyLocomotionManager>();
        }


        #region Animation Events

        public override void DamageToCone(float angle)
        {
            enemyStats.currentDamage.attackerPoint = transform.position;
            AttackManager.DamageToCone(enemyStats.currentDamage, transform.forward, transform.position,
                enemyStats.currentAttackRange, angle);
        }

        public override void DamageToSphere(float radius)
        {
            enemyStats.currentDamage.attackerPoint = transform.position;
            AttackManager.DamageToSphere(enemyStats.currentDamage, transform.position, radius);
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
            enemyManager.canBeRiposted = true;
        }

        public void DisableCanBeRiposted()
        {
            enemyManager.canBeRiposted = false;
        }

        public void EnableIsParrying()
        {
            enemyManager.isParrying = true;
        }

        public void DisableIsParrying()
        {
            enemyManager.isParrying = false;
        }

        public void EnableCanBeParried()
        {
            enemyManager.canBeParried = true;
        }

        public void DisableCanBeParried()
        {
            enemyManager.canBeParried = false;
        }

        public override void TakeCriticalDamageAnimationEvent()
        {
            if(!enemyStats.isBoss)
            {
                enemyStats.TakeDamageNoAnimation(enemyManager.pendingCriticalDamage);
                enemyManager.pendingCriticalDamage = 0;
            }
        }

        public void AwardSoulsOnDeath()
        {
            PlayerStats playerStats = FindObjectOfType<PlayerStats>();
            SoulCountBar soulCountBar = FindObjectOfType<SoulCountBar>();

            if (playerStats != null)
            {
                playerStats.AddSouls(enemyStats.soulsAwardedOnDeath);

                if (soulCountBar != null)
                {
                    soulCountBar.SetSoulCountText(playerStats.soulCount);
                }
            }
        }
        
        public void PlayWeaponTrailFX()
        {
            enemyEffectManager.PlayWeaponFX(false);
        }

        #endregion

        private void OnAnimatorMove()
        {
            //enemyManager.navMeshAgent.enabled = !enemyManager.isInteracting;
            if (enemyManager.isInteracting == false)
                return;
            
            float delta = Time.deltaTime;
            Vector3 deltaPosition = anim.deltaPosition;

            deltaPosition.y -= enemyLocomotion.ignoreGravity ? 0 : 9.81f * Time.deltaTime;

            enemyManager.controller.Move(deltaPosition);
            enemyManager.navMeshAgent.velocity = enemyManager.controller.velocity;

            if(enemyManager.isRotatingWithRootMotion)
            {
                enemyManager.transform.rotation *= anim.deltaRotation;
            }
        }
    }
}

