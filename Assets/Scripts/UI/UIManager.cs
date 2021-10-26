using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class UIManager : MonoBehaviour
    {
        PlayerInventory playerInventory;
        public EquipmentUI equipmentUI;

        [Header("UI Windows")]
        public GameObject selectWindow;
        public GameObject hudWindow;
        public GameObject inventoryWindow;
        public GameObject equipmentWindow;
        public GameObject quickSlot;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool rightHandSlot03Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        public bool leftHandSlot03Selected;
        public bool consumableSlot01Selected;
        public bool consumableSlot02Selected;
        public bool consumableSlot03Selected;

        [Header("Weapon Inventory")]
        public GameObject weaponSlotPrefab;
        public GameObject consumableSlotPrefab;
        public Transform weaponInventorySlotParent;
        public Transform consumableInventorySlotParent;
        WeaponSlot[] weaponSlots;
        ConsumableSlot[] consumableSlots;

        private void Awake()
        {
            playerInventory = GetComponent<PlayerInventory>();
        }

        private void Start()
        {
            weaponSlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponSlot>();
            consumableSlots = consumableInventorySlotParent.GetComponentsInChildren<ConsumableSlot>();
        }

        public void UpdateUI() 
        {
            #region Weapon Slots

            for (int i = 0; i < weaponSlots.Length; i++)
            {
                if (i < playerInventory.weaponsInventory.Count)
                {
                    if (weaponSlots.Length < playerInventory.weaponsInventory.Count)
                    {
                        Instantiate(weaponSlotPrefab, weaponInventorySlotParent);
                        weaponSlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponSlot>();
                    }
                    weaponSlots[i].AddItem(playerInventory.weaponsInventory[i]);
                    if (playerInventory.weaponsInventory[i].isEquiped)
                        weaponSlots[i].equiped.gameObject.SetActive(true);
                    else
                        weaponSlots[i].equiped.gameObject.SetActive(false);
                }
                else
                {
                    weaponSlots[i].ClearInventorySlot();
                }
            }
            #endregion

            #region Consumable Slots

            for (int i = 0; i < consumableSlots.Length; i++)
            {
                if (i < playerInventory.consumablesInventory.Count)
                {
                    if (consumableSlots.Length < playerInventory.consumablesInventory.Count)
                    {
                        Instantiate(consumableSlotPrefab, consumableInventorySlotParent);
                        consumableSlots = consumableInventorySlotParent.GetComponentsInChildren<ConsumableSlot>();
                    }
                    consumableSlots[i].AddItem(playerInventory.consumablesInventory[i]);
                }
                else
                {
                    consumableSlots[i].ClearInventorySlot();
                }
            }

            #endregion
        }

        public void OpenSelectWindow()
        {
            selectWindow.SetActive(true);
        }

        public void CloseSelectWindow()
        {
            selectWindow.SetActive(false);
        }

        public void CloseAllInventoryWindows()
        {
            inventoryWindow.SetActive(false);
            equipmentWindow.SetActive(false);
        }

        public void ResetAllSelectedSlots()
        {
            rightHandSlot01Selected = false;
            rightHandSlot02Selected = false;
            rightHandSlot03Selected = false;
            leftHandSlot01Selected = false;
            leftHandSlot02Selected = false;
            leftHandSlot03Selected = false;
            consumableSlot01Selected = false;
            consumableSlot02Selected = false;
            consumableSlot03Selected = false;
        }
    }
}