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
        public bool canBeParried;
        public bool isParrying;
        public bool isBlocking;

        [Header("Movement Flags")]
        public bool isRotatingWithRootMotion;
        public bool canRotate;

        public bool isInteracting;
        public bool isInvulnerable;
        public int pendingCriticalDamage;
    }
}

