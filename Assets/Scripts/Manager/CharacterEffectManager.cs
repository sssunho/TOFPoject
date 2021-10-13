using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class CharacterEffectManager : MonoBehaviour
    {
        public WeaponFX rightWeaponFX;
        public WeaponFX leftWeaponFX;



        public virtual void PlayWeaponFX(bool isLeft)
        {
            if(!isLeft)
            {
                // Play the right weapon trails
                if(rightWeaponFX != null)
                {
                    rightWeaponFX.PlayWeaponFX();
                }
            }
            else
            {
                // Play the left weapon trails
                if(leftWeaponFX != null)
                {
                    leftWeaponFX.PlayWeaponFX();
                }
            }
        }
    }
}
