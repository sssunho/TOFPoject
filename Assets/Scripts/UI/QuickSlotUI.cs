using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class QuickSlotUI : MonoBehaviour
    {
        public Image SpellIcon;
        public Image leftWeaponIcon;
        public Image rightWeaponIcon;
        public Image ConsumableIcon;

        public void UpdateWeaponQuickSlot(bool isLeft, WeaponItem weapon)
        {
            if (!isLeft)
            {
                if (weapon.itemIcon==null)
                {
                    rightWeaponIcon.sprite = null;
                    rightWeaponIcon.enabled = false;
                }
                else
                {
                    rightWeaponIcon.sprite = weapon.itemIcon;
                    rightWeaponIcon.enabled = true;
                }
            }
            else
            {
                if (weapon.itemIcon == null)
                {
                    leftWeaponIcon.sprite = null;
                    leftWeaponIcon.enabled = false;
                }
                else
                {
                    leftWeaponIcon.sprite = weapon.itemIcon;
                    leftWeaponIcon.enabled = true;
                }
            }
        }

        public void UpdateSpellQuickSlot(SpellItem spell)
        {
            if(spell.itemIcon != null)
            {
                SpellIcon.sprite = spell.itemIcon;
            }
        }

        public void UpdateConsumableQuickSlot(ConsumableItem consumable)
        {
            if(consumable.itemIcon!=null)
            {
                ConsumableIcon.sprite = consumable.itemIcon;
            }
        }
    }
}

