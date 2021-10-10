using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PlayerAttacker : MonoBehaviour
    {
        AnimatorHandler animatorHandler;
        InputHandler inputHandler;
        WeaponSlotManager weaponSlotManager;
        public string lastAttack;

        private void Awake()
        {
            animatorHandler = GetComponentInChildren<AnimatorHandler>();
            weaponSlotManager = GetComponentInChildren<WeaponSlotManager>();
            inputHandler = GetComponent<InputHandler>();
        }

        public void HandleWeaponCombo(WeaponItem weapon)
        {
            if(inputHandler.comboFlag)
            {
                animatorHandler.anim.SetBool("canDoCombo", false);

                if (lastAttack == weapon.OH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_2, true);
                }
                else if(lastAttack == weapon.TH_Light_Attack_1)
                {
                    animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_2, true);
                }
            }
        }

        public void HandleLightAttack(WeaponItem weapon)
        {
            // ep 29에서 바뀐 부분입니다.
            // weaponSlotManager.attackingWeapon = weapon; 를 추가할 때
            // 이 주석을 지우고 추가하면 됩니다.
            weaponSlotManager.attackingWeapon = weapon;
            if(inputHandler.twoHandFlag)
            {
                animatorHandler.PlayTargetAnimation(weapon.TH_Light_Attack_1, true);
                lastAttack = weapon.TH_Light_Attack_1;
            }
            else
            {
                animatorHandler.PlayTargetAnimation(weapon.OH_Light_Attack_1, true);
                lastAttack = weapon.OH_Light_Attack_1;
            }
        }

        public void HandleHeavyAttack(WeaponItem weapon)
        {
            // ep 29에서 바뀐 부분입니다.
            // weaponSlotManager.attackingWeapon = weapon; 를 추가할 때
            // 이 주석을 지우고 추가하면 됩니다.

            weaponSlotManager.attackingWeapon = weapon;
            if (inputHandler.twoHandFlag)
            {

            }
            else
            {

            }
            animatorHandler.PlayTargetAnimation(weapon.OH_Heavy_Attack_1, true);
            lastAttack = weapon.OH_Heavy_Attack_1;
        }
    }
}
