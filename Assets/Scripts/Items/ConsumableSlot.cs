using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class ConsumableSlot : MonoBehaviour
    {
        PlayerInventory playerInventory;
        ConsumableSlotManager consumableSlotManager;
        UIManager uiManager;
        public Image icon;
        public Image equiped;
        ConsumableItem item;
        EquipmentUI equipmentUI;

        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            consumableSlotManager = FindObjectOfType<ConsumableSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
            equipmentUI = FindObjectOfType<EquipmentUI>();
        }

        public void AddItem(ConsumableItem newItem)
        {
            item = newItem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearInventorySlot()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void EquipThisItem()
        {
            if (uiManager.rightHandSlot01Selected)
            {
                playerInventory.consumablesInventory.Add(playerInventory.consumablesSlot[0]);
                playerInventory.consumablesSlot[0] = item;
                playerInventory.consumablesInventory.Remove(item);
            }
            else if (uiManager.rightHandSlot02Selected)
            {
                playerInventory.consumablesInventory.Add(playerInventory.consumablesSlot[1]);
                playerInventory.consumablesSlot[1] = item;
                playerInventory.consumablesInventory.Remove(item);
            }
            else if (uiManager.rightHandSlot03Selected)
            {
                playerInventory.consumablesInventory.Add(playerInventory.consumablesSlot[2]);
                playerInventory.consumablesSlot[2] = item;
                playerInventory.consumablesInventory.Remove(item);
            }
            else return;

            playerInventory.currentConsumable = playerInventory.consumablesSlot[playerInventory.curConsumableIndex];
            consumableSlotManager.LoadConsumableOnSlot(playerInventory.currentConsumable);

            uiManager.equipmentUI.LoadConsumablesOnEquipmentScreen(playerInventory);
            uiManager.ResetAllSelectedSlots();
            equipmentUI.LoadEquipmentsIcon();
        }
    }
}