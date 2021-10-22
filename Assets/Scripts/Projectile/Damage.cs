using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    /// <summary>
    ///  SMALL : no interacting, only small reaction
    ///  NORMAL, BIG : interacting
    ///  KNOCKBACK : interacting, knock back character with rootmotion
    ///  DOWN : interacting, knock down character
    /// </summary>
    public enum HitReaction { NONE, SMALL, NORMAL, BIG, KNOCKBACK, DOWN }

    public class Damage
    {

        public int value;
        public HitReaction reaction;
        public Vector3 hitPosition;
        public Quaternion hitRotation;

    }
}
