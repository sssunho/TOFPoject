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
        WeaponItem item;

        private void Awake()
        {
            playerInventory = FindObjectOfType<PlayerInventory>();
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
            uiManager = FindObjectOfType<UIManager>();
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
            Debug.Log("equipitem");
            if (uiManager.rightHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[0]);
                playerInventory.weaponsInRightHandSlot[0] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uiManager.rightHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[1]);
                playerInventory.weaponsInRightHandSlot[1] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uiManager.rightHandSlot03Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInRightHandSlot[2]);
                playerInventory.weaponsInRightHandSlot[2] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uiManager.leftHandSlot01Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[0]);
                playerInventory.weaponsInLeftHandSlot[0] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uiManager.leftHandSlot02Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[1]);
                playerInventory.weaponsInLeftHandSlot[1] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else if (uiManager.leftHandSlot03Selected)
            {
                playerInventory.weaponsInventory.Add(playerInventory.weaponsInLeftHandSlot[2]);
                playerInventory.weaponsInLeftHandSlot[2] = item;
                playerInventory.weaponsInventory.Remove(item);
            }
            else return;

            playerInventory.rightWeapon = playerInventory.weaponsInRightHandSlot[playerInventory.curRightWeaponIndex];
            playerInventory.leftWeapon = playerInventory.weaponsInLeftHandSlot[playerInventory.curLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(playerInventory.leftWeapon, true);

            uiManager.equipmentUI.LoadWeaponsOnEquipmentScreen(playerInventory);
            uiManager.ResetAllSelectedSlots();
        }
    }
}

