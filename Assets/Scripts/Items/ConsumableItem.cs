using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class ConsumableItem : Item
    {
        [Header("Item Quantity")]
        public int maxItemAmount;
        public int currentItemAmount;

        [Header("Item Model")]
        public GameObject itemModel;

        [Header("Animations")]
        public string consumableAnimation;
        public bool isInteracting;

        public virtual void AttempToConsumeItem(AnimatorHandler animatorHandler, WeaponSlotManager weaponSlotManager, PlayerEffectManager playerEffectManager)
        {
            if(currentItemAmount >0)
            {
                animatorHandler.PlayTargetAnimation(consumableAnimation, isInteracting, true);
            }
            else
            {
                animatorHandler.PlayTargetAnimation("Shrug", true);
            }
        }
    }
}

