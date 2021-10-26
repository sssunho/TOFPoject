using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class WeaponSlot : MonoBehaviour
    {
        PlayerInventory playerInventory;
        WeaponSlotManager weaponSlotManager;
        UIManager uiManager;
        public Image icon;
        public Image equiped;
        WeaponItem item;
        EquipmentUI equipmentUI;

        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
            equipmentUI = FindObjectOfType<EquipmentUI>();
        }

        public void AddItem(WeaponItem newItem)
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
            if (item.isEquiped) return;
            if (uiManager.rightHandSlot01Selected)
            {
                if (!item.isShieldWeapon)
                    playerInventory.weaponsInRightHandSlot[0] = item;
            }
            else if (uiManager.rightHandSlot02Selected)
            {
                if (!item.isShieldWeapon)
                    playerInventory.weaponsInRightHandSlot[1] = item;
            }
            else if (uiManager.rightHandSlot03Selected)
            {
                if (!item.isShieldWeapon)
                    playerInventory.weaponsInRightHandSlot[2] = item;
            }
            else if (uiManager.leftHandSlot01Selected)
            {
                playerInventory.weaponsInLeftHandSlot[0] = item;
            }
            else if (uiManager.leftHandSlot02Selected)
            {
                playerInventory.weaponsInLeftHandSlot[1] = item;
            }
            else if (uiManager.leftHandSlot03Selected)
            {
                playerInventory.weaponsInLeftHandSlot[2] = item;
            }
            else return;

            playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlot[playerInventory.curRightWeaponIndex];
            playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlot[playerInventory.curLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);
            playerInventory.CheckEquiped();

            uiManager.equipmentUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            uiManager.ResetAllSelectedSlots();
            equipmentUI.LoadEquipmentsIcon();
        }
    }
}

