using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class EnemyWeaponSlotManager : MonoBehaviour
    {
        //public WeaponItem rightHandWeapon;
        //public WeaponItem leftHandWeapon;

        //WeaponHolderSlot rightHandSlot;
        //WeaponHolderSlot leftHandSlot;

        //DamageCollider leftHandDamageCollider;
        //DamageCollider rightHandDamageCollider;

        //private void Awake()
        //{
        //    WeaponHolderSlot[] weaponHolderSlots = GetComponentInChildren<WeaponHolderSlot>();
        //    foreach(WeaponHolderSlot weaponSlot in weaponHolderSlots)
        //    {
        //        if(weaponSlot.isLefthandSlot)
        //        {
        //            leftHandSlot = weaponSlot;
        //        }
        //        else if(weaponSlot.isRightHandSlot)
        //        {
        //            rightHandSlot = weaponSlot;
        //        }
        //    }
        //}

        //private void Start()
        //{
        //    LoadWeaponsOnBothHands();
        //}

        //public void LoadWeaponOnSlot(WeaponItem weapon, bool isLeft)
        //{
        //    if(isLeft)
        //    {
        //        leftHandSlot.currentWeapon = weapon;
        //        leftHandSlot.LoadWeaponModel(weapon);
        //        // load weapon damage collider
        //        LoadWeaponsDamageCollider(true);
        //    }
        //    else
        //    {
        //        rightHandSlot.currentWeapon = weapon;
        //        rightHandSlot.LoadWeaponModel(weapon);
        //        // load weapon damage collider
        //        LoadWeaponsDamageCollider(false);
        //    }
        //}

        //public void LoadWeaponsOnBothHands()
        //{
        //    if (rightHandWeapon != null)
        //    {
        //        LoadWeaponOnSlot(rightHandWeapon, false);
        //    }
        //    if (leftHandWeapon != null)
        //    {
        //        LoadWeaponOnSlot(leftHandWeapon, true);
        //    }
        //}

        //public void LoadWeaponsDamageCollider(bool isLeft)
        //{
        //    if(isLeft)
        //    {
        //        leftHandDamageCollider = leftHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        //    }
        //    else
        //    {
        //        rightHandDamageCollider = rightHandSlot.currentWeaponModel.GetComponentInChildren<DamageCollider>();
        //    }
        //}

        //public void OpenDamageCollider()
        //{
        //    rightHandDamageCollider.EnableDamageCollider();
        //}

        //public void CloseDamageCollider()
        //{
        //    rightHandDamageCollider.DisableDamageCollider();
        //}
    }
}