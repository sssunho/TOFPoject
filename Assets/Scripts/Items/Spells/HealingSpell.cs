using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    [CreateAssetMenu(menuName = "Spells/Healing Spell")]
    public class HealingSpell : SpellItem
    {
        public int healAmount;

        public override void AttemptToCastSpell(PlayerAnimationManager playerAnimationHandler, PlayerStats playerStats)
        {
            base.AttemptToCastSpell(playerAnimationHandler, playerStats);
            GameObject instantiatedWarmUpSpellFX = Instantiate(spellWarmUpFX, playerAnimationHandler.transform);
            playerAnimationHandler.PlayTargetAnimation(spellAnimation, true);
            Debug.Log("Attempting to cast spell...");
        }

        public override void SuccessfullyCastSpell(PlayerAnimationManager playerAnimationHandler, PlayerStats playerStats)
        {
            base.SuccessfullyCastSpell(playerAnimationHandler, playerStats);
            GameObject instantiatedSpellFX = Instantiate(spellCastFX, playerAnimationHandler.transform);
            playerStats.HealPlayer(healAmount);
            Debug.Log("Spell cast Successful!");
        }
    }
}

