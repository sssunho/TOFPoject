using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class WeaponFX : MonoBehaviour
    {
        [Header("Weapon FX")]
        public ParticleSystem normalWeaponTrail;
        // fire
        // dark
        // lightning

        public void PlayWeaponFX()
        {
            if(normalWeaponTrail.isStopped)
            {
                normalWeaponTrail.Play();
            }
            else
                normalWeaponTrail.Stop();
        }
    }
}

