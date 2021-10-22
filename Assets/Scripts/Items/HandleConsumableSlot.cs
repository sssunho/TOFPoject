using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TOF
{
    public class HandleConsumableSlot : MonoBehaviour
    {
        UIManager uiManager;
        public Image icon;
        ConsumableItem item;

        public bool consumableSlot01;
        public bool consumableSlot02;
        public bool consumableSlot03;

        private void Awake()
        {
            uiManager = FindObjectOfType<UIManager>();
        }
        public void AddItem(ConsumableItem newitem)
        {
            item = newitem;
            icon.sprite = item.itemIcon;
            icon.enabled = true;
            gameObject.SetActive(true);
        }

        public void ClearItem()
        {
            item = null;
            icon.sprite = null;
            icon.enabled = false;
            gameObject.SetActive(false);
        }

        public void SelectThisSlot()
        {
            if (consumableSlot01)
            {
                uiManager.consumableSlot01Selected = true;
            }
            else if (consumableSlot02)
            {
                uiManager.consumableSlot02Selected = true;
            }
            else
            {
                uiManager.consumableSlot03Selected = true;
            }
        }
    }
}

