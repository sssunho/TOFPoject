using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PlayerEffectManager : CharacterEffectManager
    {
        PlayerStats playerStats;
        WeaponSlotManager weaponSlotManager;
        public GameObject currentParticleFX;
        public GameObject instantiatedFXModel;
        public int amountToBeHealed;

        private void Awake()
        {
            playerStats = GetComponentInParent<PlayerStats>();
            weaponSlotManager = GetComponent<WeaponSlotManager>();
        }

        public void HealPlayerFromEffect()
        {
            playerStats.HealPlayer(amountToBeHealed);
            GameObject healParticle = Instantiate(currentParticleFX, playerStats.transform);
            Destroy(instantiatedFXModel.gameObject, 0.5f);
            weaponSlotManager.LoadBotWeaponsOnSlot();
        }
    }
}

