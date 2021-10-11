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

        [Header("Damage")]
        public int baseDamage = 25;
        public int criticalDamageMultiplier = 4;

        [Header("Idle Animations")]
        public string right_hand_idle;
        public string left_hand_idle;
        public string th_idle;

        [Header("One Handed Attak Animation")]
        public string OH_Light_Attack_1;
        public string OH_Light_Attack_2;
        public string TH_Light_Attack_1;
        public string TH_Light_Attack_2;
        public string OH_Heavy_Attack_1;
        public string OH_Heavy_Attack_2;

        [Header("Stamina Cost")]
        public int baseStamina;
        public float lightAttackMultiplier;
        public float heavyAttackMultiplier;
    }
}