using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class Projectile : MonoBehaviour
    {
        public float speed = 20.0f;
        public int teamID = 0;
        protected Damage damage;

        protected virtual void Awake()
        {
            damage.value = 10;
        }

        protected virtual void Update()
        {
            transform.Translate(transform.forward * speed * Time.deltaTime);
            damage.hitPoint = transform.position;
            damage.attackerPoint = transform.position;
        }

        private void OnTriggerEnter(Collider other)
        {
            var stat = other.GetComponent<CharacterStats>();

            if (stat == null) return;
            if (teamID == stat.teamIDNumber) return;

            stat.TakeDamage(damage);

        }
    }
}