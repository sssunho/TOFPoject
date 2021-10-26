using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class DamageCollider : MonoBehaviour
    {
        public CharacterStats senderStat;
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
            CharacterStats recieverStat = collision.GetComponent<CharacterStats>();

            if (recieverStat == null) return;
            if (teamIDNumber == recieverStat.teamIDNumber) return;

            CharacterManager manager = collision.GetComponent<CharacterManager>();
            CharacterEffectManager effect = collision.GetComponentInChildren<CharacterEffectManager>();
            BlockingCollider shield = collision.GetComponentInChildren<BlockingCollider>();

            float DefReductionRate = 100.0f * (float)recieverStat.Def / ((float)recieverStat.Def + 45.0f);

            Damage damage = new Damage();
            damage.value = Mathf.RoundToInt((float)senderStat.Atk * (1.0f - DefReductionRate));
            damage.attackerPoint = senderStat ? senderStat.transform.position : transform.position;
            damage.hitPoint = collision.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);
            damage.reaction = senderStat.attackReaction;
            recieverStat.TakeDamage(damage);

        }
    }
}
