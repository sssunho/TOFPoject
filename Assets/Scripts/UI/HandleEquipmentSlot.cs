using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class HandleEquipmentSlot : MonoBehaviour
    {
        UIManager uiManager;
        public Image icon;
        WeaponItem weapon;

        public bool rightHandSlot01;
        public bool rightHandSlot02;
        public bool rightHandSlot03;
        public bool leftHandSlot01;
        public bool leftHandSlot02;
        public bool leftHandSlot03;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        public void AddItem(WeaponItem newweapon)
        {
            weapon = newweapon;
            icon.sprite = weapon.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearItem()
        {
            weapon = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        //public void SelectThisSlot()
        //{
        //    if (rightHandSlot01)
        //    {
        //        uiManager.rightHandSlot01Selected = true;
        //    }
        //    else if(rightHandSlot02)
        //    {
        //        uiManager.rightHandSlot02Selected = true;
        //    }
        //    else if (rightHandSlot03)
        //    {
        //        uiManager.rightHandSlot03Selected = true;
        //    }
        //    else if (leftHandSlot01)
        //    {
        //        uiManager.leftHandSlot01Selected = true;
        //    }
        //    else if (leftHandSlot02)
        //    {
        //        uiManager.leftHandSlot02Selected = true;
        //    }
        //    else
        //    {
        //        uiManager.leftHandSlot03Selected = true;
        //    }
        //}
    }
}


