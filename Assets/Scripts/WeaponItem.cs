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
        public string OneHandedLightAttack1;
        public string OneHandedHeavyAttack1;
    }
}