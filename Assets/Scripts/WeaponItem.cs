using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace TOF
{
    [CreateAssetMenu(menuName = "Items/Weapon Item")]
    public class WeaponItem : Item
    {
        public GameObject modelPrefab;
        public bool inUnarmed;

        [Header("One Handed Attak Animation")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string OH_Heavy_Attack_1;
        public string OH_Heavy_Attack_2;
    }
}