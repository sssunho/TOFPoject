using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;

        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public WeaponItem unarmed;

        public WeaponItem[] weaponsInRightHandSlot = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlot = new WeaponItem[1];

        public int curRightWeaponIndex = 0;
        public int curLeftWeaponIndex = 0;

        public List<WeaponItem> weaponsInventory;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            rightWeapon = weaponsInRightHandSlot[0];
            leftWeapon = weaponsInLeftHandSlot[0];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
        }

        public void changeRightWeapon()
        {
            curRightWeaponIndex += 1;
            if (curRightWeaponIndex == 0 && weaponsInRightHandSlot[0] != null)
            {
                rightWeapon = weaponsInRightHandSlot[curRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlot[curRightWeaponIndex], false);
            }
            else if(curRightWeaponIndex == 0 && weaponsInRightHandSlot[0] == null)
                curRightWeaponIndex += 1;

            else if(curRightWeaponIndex == 1 && weaponsInRightHandSlot[1] != null)
            {
                rightWeapon = weaponsInRightHandSlot[curRightWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlot[curRightWeaponIndex], false);
            }
            else
                curRightWeaponIndex += 1;

            if(curRightWeaponIndex> weaponsInRightHandSlot.Length - 1)
            {
                curRightWeaponIndex = -1;
                rightWeapon = unarmed;
                weaponSlotManager.LoadWeaponOnSlot(unarmed, false);
            }
        }

        public void changeLeftWeapon()
        {
            curLeftWeaponIndex += 1;
            if (curLeftWeaponIndex == 0 && weaponsInLeftHandSlot[0] !=null) return;

            curLeftWeaponIndex = 1 - curLeftWeaponIndex;
            leftWeapon = weaponsInLeftHandSlot[curLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlot[curLeftWeaponIndex], false);
        }
    }
}
