using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public enum Direction4Way { FORWARD, RIGHT, LEFT, BEHIND }

    public class CharacterStats : MonoBehaviour
    {
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

        public void PlaySmallReaction(Direction4Way direction)
        {
            switch (direction)
            {
                case Direction4Way.FORWARD:
                    break;
                case Direction4Way.RIGHT:
                    break;
                case Direction4Way.LEFT:
                    break;
                case Direction4Way.BEHIND:
                    break;
                default:
                    break;
            }
        }
    }
}
