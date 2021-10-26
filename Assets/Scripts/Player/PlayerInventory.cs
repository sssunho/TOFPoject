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

        public WeaponItem[] weaponsInRightHandSlot;
        public WeaponItem[] weaponsInLeftHandSlot;
        public ConsumableItem[] consumablesSlot;
        public SpellItem[] spellsSlot;

        public int curRightWeaponIndex = 0;
        public int curLeftWeaponIndex = 0;
        public int curConsumableIndex = 0;
        public int curSpellIndex = 0;

        public List<WeaponItem> weaponsInventory;
        public List<ConsumableItem> consumablesInventory;
        public List<SpellItem> spellsInventory;

        //public list<weaponitem> getweaponslots() { return weaponsinventory; }
        //public list<consumableitem> getconsumableslots() { return consumablesinventory; }
        //public list<spellitem> getspellitems() { return spellsinventory; }
        //[SerializeField] private Item[] items;
        //[SerializeField] private Item[] items;

        //public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
        //{
        //    for(int i = 0; i < items.Length; i++)
        //    {
        //        if (items[i].itemName == _itemName)

        //    }
        //}

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
            if (curRightWeaponIndex == weaponsInRightHandSlot.Length)
                curRightWeaponIndex = 0;
            else
            {
                for(int i = curRightWeaponIndex; i < weaponsInRightHandSlot.Length; i++)
                {
                    if (weaponsInRightHandSlot[i] != null)
                    {
                        curRightWeaponIndex = i;
                        break;
                    }
                    else
                        curRightWeaponIndex = 0;
                }
            }
            rightWeapon = weaponsInRightHandSlot[curRightWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInRightHandSlot[curRightWeaponIndex], false);
        }

        public void changeLeftWeapon()
        {
            curLeftWeaponIndex++;
            if (curLeftWeaponIndex == weaponsInLeftHandSlot.Length)
                curLeftWeaponIndex = 0;
            else
            {
                for (int i = curLeftWeaponIndex; i < weaponsInLeftHandSlot.Length; i++)
                {
                    if (weaponsInLeftHandSlot[i] != null)
                    {
                        curLeftWeaponIndex = i;
                        break;
                    }
                    else
                        curLeftWeaponIndex = 0;
                }
            }
            leftWeapon = weaponsInLeftHandSlot[curLeftWeaponIndex];
            weaponSlotManager.LoadWeaponOnSlot(weaponsInLeftHandSlot[curLeftWeaponIndex], true);
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
