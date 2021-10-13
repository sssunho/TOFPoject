using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class CharacterEffectManager : MonoBehaviour
    {
        [Header("Damage FX")]
        public GameObject bloodSplatterFX;
        public GameObject recoilMetalFX;
        [Header("Weapon FX")]
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

        public virtual void PlayBloodSplatterFX(Vector3 bloodSplatterLocation)
        {
            GameObject blood = Instantiate(bloodSplatterFX, bloodSplatterLocation, Quaternion.identity);
        }

        public virtual void PlayRecoilMetalFX(Vector3 recoilLocation)
        {
            GameObject recoilMetal = Instantiate(recoilMetalFX, recoilLocation, Quaternion.identity);
        }

    }
}
