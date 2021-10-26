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
            SetEquips();
            weaponSlotManager.LoadWeaponOnSlot(rightWeapon, false);
            weaponSlotManager.LoadWeaponOnSlot(leftWeapon, true);
            consumableSlotManager.LoadConsumableOnSlot(currentConsumable);
        }

        public void changeRightWeapon()
        {
            curRightWeaponIndex++;
            if(curRightWeaponIndex > weaponsInRightHandSlot.Length - 1)
            {
                curRightWeaponIndex = -1;
                rightWeapon = unarmed;
                weaponSlotManager.LoadWeaponOnSlot(unarmed, false);
            }
            else
            {
                if (weaponsInRightHandSlot[curRightWeaponIndex] != null)
                {
                    rightWeapon = weaponsInRightHandSlot[curRightWeaponIndex];
                    weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlot[curRightWeaponIndex], false);
                }
                else
                    curRightWeaponIndex++;
            }
        }

        public void changeLeftWeapon()
        {
            curLeftWeaponIndex++;
            if (curLeftWeaponIndex > weaponsInLeftHandSlot.Length - 1)
            {
                curLeftWeaponIndex = -1;
                leftWeapon = unarmed;
                weaponSlotManager.LoadWeaponOnSlot(unarmed, true);
            }
            else
            {
                if (weaponsInLeftHandSlot[curLeftWeaponIndex] != null)
                {
                    leftWeapon = weaponsInLeftHandSlot[curLeftWeaponIndex];
                    weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlot[curLeftWeaponIndex], true);
                }
                else
                    curLeftWeaponIndex++;
            }
        }

        public void SetEquips()
        {
            for (int i = 0; i < 3; i++)
            {
                if (weaponsInRightHandSlot[i] != null)
                {
                    weaponsInventory.Add(weaponsInRightHandSlot[i]);
                }
                if (weaponsInLeftHandSlot[i] != null)
                {
                    weaponsInventory.Add(weaponsInLeftHandSlot[i]);
                }
                if (consumablesSlot[i] != null)
                {
                    consumablesInventory.Add(consumablesSlot[i]);
                }
            }

            CheckEquiped();
        }

        public void CheckEquiped()
        {
            for (int i = 0; i < weaponsInventory.Count; i++)
                weaponsInventory[i].isEquiped = false;
            for (int i = 0; i < consumablesInventory.Count; i++)
                consumablesInventory[i].isEquiped = false;

            for (int i = 0; i < 3; i++)
            {
                if (weaponsInRightHandSlot[i] != null)
                    weaponsInRightHandSlot[i].isEquiped = true;
                if (weaponsInLeftHandSlot[i] != null)
                    weaponsInLeftHandSlot[i].isEquiped = true;
                if (consumablesSlot[i] != null)
                    consumablesSlot[i].isEquiped = true;
            }
        }
    }
}
