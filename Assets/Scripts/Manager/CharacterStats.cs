using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public enum Direction4Way { FORWARD, RIGHT, LEFT, BEHIND }

    public class CharacterStats : MonoBehaviour
    {
        protected AnimatorManager animatorManager;
        protected CharacterManager characterManager;
        protected CharacterEffectManager effectManager;
        public Damage currentDamage = new Damage();

        public float statCoefficient = 45;
        public float DefCoefficient = 45;

        public HitReaction attackReaction;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        public int healthLevel = 100;
        public int maxHealth;
        public int currentHealth;

        public int staminaLevel = 100;
        public float maxStamina;
        public float currentStamina;

        public int focusLevel = 100;
        public float maxFocusPoints;
        public float currentFocusPoint;

        public int soulCount = 0;

        public bool isDead;

        public virtual int Atk
        {
            get
            {
                return 0;
            }
        }

        public virtual int Def
        {
            get
            {
                return 0;
            }
        }

        public virtual float Mov
        {
            get
            {
                return 1.0f;
            }
        }
        public virtual float Crit
        {
            get
            {
                return 0;
            }
        }

        public float maxPoise = 100;
        public float curPoise = 100;
        public float poiseRecoveryRate = 30;
        bool poiseRecovery = true;
        Coroutine poiseCoroutine = null;

        private void Awake()
        {
            effectManager = GetComponentInChildren<CharacterEffectManager>();
        }

        protected virtual void Update()
        {
            if (poiseRecovery)
            {
                if (curPoise < maxPoise)
                {
                    curPoise = Mathf.Clamp(curPoise + poiseRecoveryRate * Time.deltaTime, 0, maxPoise);
                }
            }
        }

        public virtual void TakeDamage(Damage damage)
        {
            if (isDead) return;

            Vector3 rel = damage.attackerPoint - transform.position;
            rel.y = 0;
            float angle = Vector3.Angle(rel, transform.forward);
            Vector3 cross = Vector3.Cross(rel, transform.forward);
            if (cross.y > 0) angle = -angle;
            Direction4Way hitDirection = AngleToDirection4(angle);

            if (hitDirection == Direction4Way.FORWARD && characterManager.isBlocking && !damage.ignoreGuard)
            {
                BlockingCollider shield = transform.GetComponentInChildren<BlockingCollider>();
                if (shield != null)
                {
                    damage.reaction = HitReaction.GUARD;
                    damage.value *= Mathf.RoundToInt((1.0f - shield.blockingPhysicalDamageAbsorption));
                    effectManager.PlayRecoilMetalFX(damage.hitPoint);
                }
            }
            else
            {
                effectManager.PlayBloodSplatterFX(damage.hitPoint);
            }

            currentHealth -= damage.value;

            if (damage.poiseDamage > 0)
            {
                poiseRecovery = false;
                curPoise = Mathf.Clamp(curPoise - damage.poiseDamage, 0, maxPoise);
                curPoise = Mathf.Clamp(curPoise - 20, 0, maxPoise);
                if (poiseCoroutine != null)
                    StopCoroutine(ResetPoiseRecovery());
                poiseCoroutine = StartCoroutine(ResetPoiseRecovery());
            }

            PlayHitReaction(damage, hitDirection);

            if (damage.reaction == HitReaction.KNOCKBACK)
                transform.rotation = Quaternion.LookRotation(rel);

            if (currentHealth <= 0)
            {
                animatorManager.PlayTargetAnimation("Dead_01", true);
                isDead = true;
            }
        }

        private void PlayHitReaction(Damage damage, Direction4Way hitDirection)
        {
            if (damage.reaction == HitReaction.NONE) return;
            if (currentHealth <= 0) return;

            //if (curPoise > 0)
            //{
            //    if (damage.reaction == HitReaction.NORMAL ||
            //        damage.reaction == HitReaction.BIG)

            //        return;
            //}

            animatorManager.anim.SetInteger("reactionDirection", (int)hitDirection);
            animatorManager.anim.SetInteger("reactionID", (int)damage.reaction);
            animatorManager.anim.SetTrigger("reactionTrigger");

            animatorManager.SetInteraction(damage.reaction != HitReaction.SMALL);

        }

        private void PlayHitEffect(Damage damage)
        {

        }

        protected Direction4Way AngleToDirection4(float angle)
        {
            if (angle > -45 && angle <= 45)
                return Direction4Way.FORWARD;
            else if (angle > -135 && angle <= -45)
                return Direction4Way.LEFT;
            else if (angle > 45 && angle <= 135)
                return Direction4Way.RIGHT;
            else
                return Direction4Way.BEHIND;
        }

        IEnumerator ResetPoiseRecovery()
        {
            yield return new WaitForSeconds(1.0f);
            poiseRecovery = true;
            poiseCoroutine = null;
        }
    }
}
