using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;
        public EquipmentUI equipmentUI;

        [Header("UI Windows")]
        public GameObject selectWindow;
        public GameObject hudWindow;
        public GameObject weaponInventoryWindow;
        public GameObject equipmentWindow;

        [Header("Equipment Window Slot Selected")]
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool rightHandSlot03Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        public bool leftHandSlot03Selected;

        [Header("Weapon Inventory")]
        public GameObject weaponSlotPrefab;
        public Transform weaponInventorySlotParent;
        WeaponSlot[] weaponSlots;

        private void Awake()
        {
            equipmentUI = FindObjectOfType<EquipmentUI>();
        }

        private void Start()
        {
            weaponSlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponSlot>();
            equipmentUI.LoadWeaponsOnEquipmentScreen(playerInventory);
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
                }
                else
                {
                    weaponSlots[i].ClearInventorySlot();
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
            weaponInventoryWindow.SetActive(false);
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
        }
    }
}