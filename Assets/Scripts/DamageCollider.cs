using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterManager characterManager;
        protected Collider damageCollider;
        public int currentWeaponDamage = 25;

        protected virtual void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;

            characterManager = GetComponentInParent<CharacterManager>();
        }


        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        protected virtual void OnTriggerEnter(Collider collision)
        {
            if (collision.tag == "Player")
            {
                PlayerStats playerStats = collision.GetComponent<PlayerStats>();
                CharacterManager playerCharacterManager = collision.GetComponent<CharacterManager>();
                CharacterEffectManager playerEffectManager = collision.GetComponentInChildren<CharacterEffectManager>();
                BlockingCollider shield = collision.transform.GetComponentInChildren<BlockingCollider>();

                if (playerCharacterManager != null)
                {
                    if (shield != null && playerCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                        if (playerStats != null)
                        {
                            Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                            playerEffectManager.PlayRecoilMetalFX(contactPoint);
                            playerStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                        }
                    }
                    else if (playerStats != null)
                    {
                        // Detects where on the collider our weapon first makes contact
                        Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                        playerEffectManager.PlayBloodSplatterFX(contactPoint);
                        playerStats.TakeDamage(currentWeaponDamage);
                    }
                }
            }

            if (collision.tag == "Enemy")
            {
                EnemyStats enemyStats = collision.GetComponentInParent<EnemyStats>();
                CharacterManager enemyCharacterManager = collision.GetComponentInParent<CharacterManager>();
                CharacterEffectManager enemyEffectManager = enemyStats.GetComponentInChildren<CharacterEffectManager>();
                BlockingCollider shield = collision.transform.GetComponent<BlockingCollider>();

                if (enemyCharacterManager != null)
                {
                    if (shield != null && enemyCharacterManager.isBlocking)
                    {
                        float physicalDamageAfterBlock = currentWeaponDamage - (currentWeaponDamage * shield.blockingPhysicalDamageAbsorption) / 100;

                        if (enemyStats != null)
                        {
                            Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                            enemyEffectManager.PlayRecoilMetalFX(contactPoint);
                            enemyStats.TakeDamage(Mathf.RoundToInt(physicalDamageAfterBlock), "Block Guard");
                        }
                    }

                    if (enemyStats != null)
                    {
                        // Detects where on the collider our weapon first makes contact
                        Vector3 contactPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
                        enemyEffectManager.PlayBloodSplatterFX(contactPoint);
                        enemyStats.TakeDamage(currentWeaponDamage);
                    }
                }

            }
        }
    }
}
