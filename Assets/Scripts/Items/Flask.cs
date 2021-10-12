using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    [CreateAssetMenu(menuName = "Items/Consumables/Flask")]
    
    public class Flask : ConsumableItem
    {
        [Header("Plask Type")]
        public bool estusFlask;
        public bool ashenFlask;

        [Header("Recovery Amount")]
        public int healthRecoverAmount;
        public int focusPointsRecoverAmount;

        [Header("Recovery FX")]
        public GameObject recoveryFX;

        public override void AttempToConsumeItem(AnimatorManager animatorHandler, WeaponSlotManager weaponSlotManager, PlayerEffectManager playerEffectManager)
        {
            base.AttempToConsumeItem(animatorHandler, weaponSlotManager, playerEffectManager);
            GameObject flask = Instantiate(itemModel, weaponSlotManager.rightHandSlot.transform);
            playerEffectManager.currentParticleFX = recoveryFX;
            playerEffectManager.amountToBeHealed = healthRecoverAmount;
            playerEffectManager.instantiatedFXModel = flask;
            weaponSlotManager.rightHandSlot.UnloadWeapon();
        }
    }
}

