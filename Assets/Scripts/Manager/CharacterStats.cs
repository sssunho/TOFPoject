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

        int poise;

        private void Awake()
        {
            animatorManager = GetComponentInChildren<AnimatorManager>();
            characterManager = GetComponent<CharacterManager>();
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

            if(hitDirection == Direction4Way.FORWARD && characterManager.isBlocking)
            {
                BlockingCollider shield = transform.GetComponentInChildren<BlockingCollider>();
                damage.reaction = HitReaction.GUARD;
                damage.value *= Mathf.RoundToInt((1.0f - shield.blockingPhysicalDamageAbsorption));
            }

            currentHealth -= damage.value; 

            PlayHitReaction(damage, hitDirection);

            if(currentHealth <= 0)
            {
                animatorManager.PlayTargetAnimation("Dead_01", true);
                isDead = true;
            }
        }

        private void PlayHitReaction(Damage damage, Direction4Way hitDirection)
        {
            if (damage.reaction == HitReaction.NONE) return;

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
    }
}
