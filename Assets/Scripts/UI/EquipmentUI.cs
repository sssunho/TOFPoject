using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class EquipmentUI : MonoBehaviour
    {
        public bool rightHandSlot01Selected;
        public bool rightHandSlot02Selected;
        public bool rightHandSlot03Selected;
        public bool leftHandSlot01Selected;
        public bool leftHandSlot02Selected;
        public bool leftHandSlot03Selected;

        public HandleEquipmentSlot[] handleEquipmentSlots;

        public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
        {
            for (int i = 0; i < handleEquipmentSlots.Length; i++)
            {
                if(handleEquipmentSlots[i].rightHandSlot01)
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
            rightHandSlot01Selected = true;
        }

        public void SelectRightHandSlot02()
        {
            rightHandSlot02Selected = true;
        }

        public void SelectRightHandSlot03()
        {
            rightHandSlot03Selected = true;
        }

        public void SelectLeftHandSlot01()
        {
            leftHandSlot01Selected = true;
        }

        public void SelectLeftHandSlot02()
        {
            leftHandSlot02Selected = true;
        }

        public void SelectLeftHandSlot03()
        {
            leftHandSlot03Selected = true;
        }
    }
}