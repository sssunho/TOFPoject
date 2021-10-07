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

        public WeaponItem[] weaponsInRightHandSlot = new WeaponItem[2];
        public WeaponItem[] weaponsInLeftHandSlot = new WeaponItem[2];

        public int curRightWeaponIndex = 0;
        public int curLeftWeaponIndex = 0;

        private void Awake()
        {
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
        }

        private void Start()
        {
            //weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            //weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            //rightWeapon = weaponsInRightHandSlot[curRightWeaponIndex];
            //leftWeapon = weaponsInLeftHandSlot[curLeftWeaponIndex];
            rightWeapon = unarmed;
            leftWeapon = unarmed;
        }

        public void changeRightWeapon()
        {
            if (weaponsInRightHandSlot[1 - curRightWeaponIndex] == null) return;

            curRightWeaponIndex = 1 - curRightWeaponIndex;
            rightWeapon = weaponsInRightHandSlot[curRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlot[curRightWeaponIndex], false);
        }

        public void changeLeftWeapon()
        {
            if (weaponsInLeftHandSlot[1 - curLeftWeaponIndex] == null) return;

            curLeftWeaponIndex = 1 - curLeftWeaponIndex;
            leftWeapon = weaponsInLeftHandSlot[curLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlot[curLeftWeaponIndex], false);
        }
    }
}
