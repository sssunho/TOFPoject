using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class CharacterManager : MonoBehaviour
    {
        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        [Header("Combat Colliders")]
        public CriticalDamageCollider backStabCollider;
        public CriticalDamageCollider riposteCollider;

        [Header("Combat Flags")]
        public bool canBeRiposted;
<<<<<<< HEAD
        public bool canBeParried;
        public bool isParrying;
=======
        public bool isBlocking;
>>>>>>> 037d88d0b35cc7e970ec93e546982ef5c58f48f4

        public int pendingCriticalDamage;
    }
}

