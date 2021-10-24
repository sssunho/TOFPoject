using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PlayerInventory : MonoBehaviour
    {
        WeaponSlotManager weaponSlotManager;
        ConsumableSlotManager consumableSlotManager;

        public SpellItem currentSpell;
        public WeaponItem rightWeapon;
        public WeaponItem leftWeapon;
        public ConsumableItem currentConsumable;
        public WeaponItem unarmed;

        public WeaponItem[] weaponsInRightHandSlot = new WeaponItem[1];
        public WeaponItem[] weaponsInLeftHandSlot = new WeaponItem[1];
        public ConsumableItem[] consumablesSlot = new ConsumableItem[1];
        public SpellItem[] spellsSlot = new SpellItem[1];

        public int curRightWeaponIndex = 0;
        public int curLeftWeaponIndex = 0;
        public int curConsumableIndex = 0;
        public int curSpellIndex = 0;

        public List<WeaponItem> weaponsInventory;
        public List<ConsumableItem> consumablesInventory;
        public List<SpellItem> spellsInventory;

        private void Awake()
        {
            weaponSlotManager = FindObjectOfType<WeaponSlotManager>();
            consumableSlotManager = FindObjectOfType<ConsumableSlotManager>();
        }

        private void Start()
        {
            rightWeapon = weaponsInRightHandSlot[0];
            leftWeapon = weaponsInLeftHandSlot[0];
            currentConsumable = consumablesSlot[0];
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            consumableSlotManager.LoadConsumableOnSlot(currentConsumable);
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
            if (curLeftWeaponIndex == 0 && weaponsInLeftHandSlot[0] != null)
            {
                leftWeapon = weaponsInLeftHandSlot[curLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlot[curLeftWeaponIndex], false);
            }
            else if (curLeftWeaponIndex == 0 && weaponsInLeftHandSlot[0] == null)
                curLeftWeaponIndex += 1;

            else if (curLeftWeaponIndex == 1 && weaponsInLeftHandSlot[1] != null)
            {
                leftWeapon = weaponsInLeftHandSlot[curLeftWeaponIndex];
                weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlot[curLeftWeaponIndex], false);
            }
            else
                curLeftWeaponIndex += 1;

            if (curLeftWeaponIndex > weaponsInLeftHandSlot.Length - 1)
            {
                curLeftWeaponIndex = -1;
                leftWeapon = unarmed;
                weaponSlotManager.LoadWeaponOnSlot(unarmed, false);
            }
        }
    }
}
