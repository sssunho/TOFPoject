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

        [HideInInspector]
        public WeaponItem[] GetRightWeaponItems() { return weaponsInRightHandSlot; }
        public WeaponItem[] GetLeftWeaponItems() { return weaponsInLeftHandSlot; }
        public ConsumableItem[] GetConsumableItems() { return consumablesSlot; }
        public SpellItem[] GetSpellItems() { return spellsSlot; }
        public List<WeaponItem> GetWeaponInventory() { return weaponsInventory; }
        public WeaponItem[] InvenWeapons;

        [SerializeField] private WeaponItem[] ALLWeapons;
        [SerializeField] private WeaponItem[] RightWeapons;
        [SerializeField] private WeaponItem[] LeftWeapons;
        [SerializeField] private ConsumableItem[] Consumables;
        [SerializeField] private SpellItem[] Spells;

        public void LoadToRightWeapon(int _arrayNum, string _itemName)
        {
            // #. Weapon(Right)
            for (int i = 0; i < RightWeapons.Length; i++)
                if (RightWeapons[i].itemName == _itemName)
                    weaponsInRightHandSlot[_arrayNum] = RightWeapons[i];
        }
        public void LoadToLefttWeapon(int _arrayNum, string _itemName)
        {
            // #. Weapon(Left)
            for (int i = 0; i < LeftWeapons.Length; i++)
                if (LeftWeapons[i].itemName == _itemName)
                    weaponsInLeftHandSlot[_arrayNum] = LeftWeapons[i];
        }
        public void LoadToConsumableItem(int _arrayNum, string _itemName)
        {
            // #. ConsumableItem
            for (int i = 0; i < Consumables.Length; i++)
                if (Consumables[i].itemName == _itemName)
                    consumablesSlot[_arrayNum] = Consumables[i];
        }
        public void LoadToSpellItem(int _arrayNum, string _itemName)
        {
            // #. SpellItem
            for (int i = 0; i < Spells.Length; i++)
                if (Spells[i].itemName == _itemName)
                    spellsSlot[_arrayNum] = Spells[i];
        }
        public void LoadToInven(int _arrayNum, string _itemName)
        {
            // #. UnEquiped Items
            for (int i = 0; i < ALLWeapons.Length; i++)
                if (ALLWeapons[i].itemName == _itemName)
                {
                    InvenWeapons[_arrayNum] = ALLWeapons[i];
                }
        }

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

            if(InvenWeapons != null)
            {
                for (int i = 0; i < InvenWeapons.Length; i++)
                {
                    weaponsInventory.Add(InvenWeapons[i]);
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
