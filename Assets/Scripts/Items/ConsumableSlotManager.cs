using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class ConsumableSlotManager : MonoBehaviour
    {
        PlayerManager playerManager;
        PlayerInventory playerInventory;

        public ConsumableItem consumable;

        Animator animator;

        QuickSlotUI quickSlotUI;

        InputHandler inputHandler;

        PlayerStats playerStats;
        PlayerEffectManager playerEffectManager;

        private void Awake()
        {
            playerManager = GetComponentInParent<PlayerManager>();
            animator = GetComponent<Animator>();
            quickSlotUI = FindObjectOfType<QuickSlotUI>();
            inputHandler = GetComponentInParent<InputHandler>();
            playerStats = GetComponentInParent<PlayerStats>();
            playerInventory = GetComponentInParent<PlayerInventory>();
            playerEffectManager = GetComponent<PlayerEffectManager>();
        }

        public void LoadBotConsumableOnSlot()
        {
            LoadConsumableOnSlot(playerInventory.currentConsumable);
        }

        public void LoadConsumableOnSlot(ConsumableItem consumable)
        {
            quickSlotUI.UpdateConsumableQuickSlot(consumable);
        }
    }
}

