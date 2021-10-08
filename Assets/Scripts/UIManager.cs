using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class UIManager : MonoBehaviour
    {
        public PlayerInventory playerInventory;

        [Header("UI Windows")]
        public GameObject selectWindow;
        public GameObject hudWindow;
        public GameObject weaponInventoryWindow;

        [Header("Weapon Inventory")]
        public GameObject weaponSlotPrefab;
        public Transform weaponInventorySlotParent;
        WeaponSlot[] weaponSlots;

        private void Start()
        {
            weaponSlots = weaponInventorySlotParent.GetComponentsInChildren<WeaponSlot>();
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
        }
    }
}