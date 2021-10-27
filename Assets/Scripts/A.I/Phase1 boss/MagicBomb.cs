using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class MagicBomb : MonoBehaviour
    {
        public GameObject effect1;
        public GameObject effect2;
        public float r = 3.0f;
        public int damage = 30;
        public bool debug = false;

        float _time = 0.0f;
        bool active = false;

        private void OnDrawGizmos()
        {
            if (debug)
                Gizmos.DrawSphere(transform.position, r);
        }

        private void Start()
        {
            EffectManager.PlayEffect(7, transform.position + Vector3.up, transform.rotation, 1.5f * Vector3.one);
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time > 1.5f && !active)
            {
                active = true;

                EffectManager.PlayEffect(0, transform.position, Quaternion.LookRotation(Vector3.up), 1.5f * Vector3.one);
                DamageSphere();
                Destroy(gameObject, 2.5f);
            }
        }

        private void DamageSphere()
        {
            var colliders = Physics.OverlapSphere(transform.position, r);
            for (int i = 0; i < colliders.Length; i++)
            {
                var stat = colliders[i].GetComponent<PlayerStats>();

                if (stat == null) continue;
                Damage damage = new Damage();
                damage.reaction = HitReaction.KNOCKBACK;
                damage.value = 30;
                damage.ignoreGuard = true;
                damage.hitPoint = transform.position;

                stat.TakeDamage(damage);
        }
        }
    }
}
