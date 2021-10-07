using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class DamageCollider : MonoBehaviour
    {
        Collider damageCollider;
        public int currentWeaponDamage = 25;

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = true;

        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
            Debug.Log("open");
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            Debug.Log("close");
        }

        private void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();

                if (playerStats != null)
                {
                    playerStats.TakeDamage(currentWeaponDamage);
                }
            }
            
            if (collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponent<EnemyStats>();

                if (enemyStats != null)
                {
                    enemyStats.TakeDamage(currentWeaponDamage);
                    Debug.Log("coll");
                }
            }
        }
    }
}
