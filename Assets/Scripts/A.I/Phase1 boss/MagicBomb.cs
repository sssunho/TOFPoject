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
            var inst = Instantiate(effect1, transform.position + Vector3.up, transform.rotation, null);
            inst.transform.localScale = 1.5f * Vector3.one;
            Destroy(inst, 2.0f);
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time > 1.5f && !active)
            {
                active = true;
                var inst = Instantiate(effect2, transform.position, Quaternion.LookRotation(Vector3.up), null);
                inst.transform.localScale = 1.5f * Vector3.one;
                Destroy(inst, 2.0f);
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
                damage.reaction = HitReaction.BIG;
                damage.value = 30;
                damage.ignoreGuard = true;
                damage.hitPoint = transform.position;

                stat.TakeDamage(damage);
        }
        }
    }
}
