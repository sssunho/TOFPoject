using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterStats characterStat;
        protected Collider damageCollider;
        public int currentWeaponDamage = 25;

        [Header("Team I.D")]
        public int teamIDNumber = 0;

        protected virtual void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
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
            CharacterStats stats = collision.GetComponent<CharacterStats>();

            if (stats == null) return;
            if (teamIDNumber == stats.teamIDNumber) return;

            CharacterManager manager = collision.GetComponent<CharacterManager>();
            CharacterEffectManager effect = collision.GetComponentInChildren<CharacterEffectManager>();
            BlockingCollider shield = collision.GetComponentInChildren<BlockingCollider>();

            Damage damage = new Damage();
            damage.value = currentWeaponDamage;
            damage.attackerPoint = characterStat.transform.position;
            damage.hitPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            damage.reaction = characterStat.attackReaction;
            stats.TakeDamage(damage);

        }
    }
}
