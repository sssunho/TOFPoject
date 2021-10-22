using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class EquipmentUI : MonoBehaviour
    {
        PlayerInventory playerInventory;
        UIManager uiManager;

        public Image[] rightHandSlotIcon;
        public Image[] leftHandSlotIcon;
        public Image[] consumableSlotIcon;

        public HandleEquipmentSlot[] handleEquipmentSlots;
        public HandleConsumableSlot[] handleConsumableSlots;

        int i;

        private void Awake()
        {
            playerInventory = GetComponentInParent<PlayerInventory>();
            uiManager = GetComponentInParent<UIManager>();
        }

        private void Start()
        {
            LoadEquipmentsIcon();
        }

        public void LoadEquipmentsIcon()
        {
            for (i = 0; i < playerInventory.weaponsInRightHandSlot.Length-1; i++)
            {
                rightHandSlotIcon[i].sprite = playerInventory.weaponsInRightHandSlot[i].itemIcon;
            }
            for (i = 0; i < playerInventory.weaponsInLeftHandSlot.Length-1; i++)
            {
                leftHandSlotIcon[i].sprite = playerInventory.weaponsInLeftHandSlot[i].itemIcon;
            }
            for (i = 0; i < playerInventory.consumablesSlot.Length-1; i++)
            {
                consumableSlotIcon[i].sprite = playerInventory.consumablesSlot[i].itemIcon;
            }
        }

        public void LoadConsumablesOnEquipmentScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handleConsumableSlots.Length-1; i++)
            {
                if(handleConsumableSlots[i].consumableSlot01)
                {
                    handleConsumableSlots[i].AddItem(playerInventory.consumablesSlot[0]);
                }
                else if (handleConsumableSlots[i].consumableSlot02)
                {
                    handleConsumableSlots[i].AddItem(playerInventory.consumablesSlot[1]);
                }
                else 
                {
                    handleConsumableSlots[i].AddItem(playerInventory.consumablesSlot[2]);
                }
            }
        }

        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handleEquipmentSlots.Length; i++)
            {
                if (handleEquipmentSlots[i].rightHandSlot01)
                {
                    handleEquipmentSlots[i].AddItem(playerInventory.weaponsInRightHandSlot[0]);
                }
                else if (handleEquipmentSlots[i].rightHandSlot02)
                {
                    handleEquipmentSlots[i].AddItem(playerInventory.weaponsInRightHandSlot[1]);
                }
                else if (handleEquipmentSlots[i].rightHandSlot03)
                {
                    handleEquipmentSlots[i].AddItem(playerInventory.weaponsInRightHandSlot[2]);
                }
                else if (handleEquipmentSlots[i].leftHandSlot01)
                {
                    handleEquipmentSlots[i].AddItem(playerInventory.weaponsInLeftHandSlot[0]);
                }
                else if (handleEquipmentSlots[i].leftHandSlot02)
                {
                    handleEquipmentSlots[i].AddItem(playerInventory.weaponsInLeftHandSlot[1]);
                }
                else
                {
                    handleEquipmentSlots[i].AddItem(playerInventory.weaponsInLeftHandSlot[2]);
                }
            }
        }

        public void SelectRightHandSlot01()
        {
            uiManager.rightHandSlot01Selected = true;
        }

        public void SelectRightHandSlot02()
        {
            uiManager.rightHandSlot02Selected = true;
        }

        public void SelectRightHandSlot03()
        {
            uiManager.rightHandSlot03Selected = true;
        }

        public void SelectLeftHandSlot01()
        {
            uiManager.leftHandSlot01Selected = true;
        }

        public void SelectLeftHandSlot02()
        {
            uiManager.leftHandSlot02Selected = true;
        }

        public void SelectLeftHandSlot03()
        {
            uiManager.leftHandSlot03Selected = true;
        }

        public void SelectConsumalbeSlot01()
        {
            uiManager.consumableSlot01Selected = true;
        }

        public void SelectConsumalbeSlot02()
        {
            uiManager.consumableSlot02Selected = true;
        }

        public void SelectConsumalbeSlot03()
        {
            uiManager.consumableSlot03Selected = true;
        }
    }
}