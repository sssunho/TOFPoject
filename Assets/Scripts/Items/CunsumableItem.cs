using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class CunsumableItem : MonoBehaviour
    {
        [Header("Item Quantity")]
        public int maxItemAmount;
        public int currentItemAmount;

        [Header("Item Model")]
        public GameObject itemModel;

        [Header("Animations")]
        public string consumableAnimation;
        public bool isInteracting;

        public virtual void AttempToConsumeItem()
        {

        }
    }
}

