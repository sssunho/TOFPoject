using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class Projectile : DamageCollider
    {
        public GameObject destroyFx;

        protected override void Awake()
        {
            base.Awake();
            damageCollider.enabled = true;
        }

        protected override void OnTriggerEnter(Collider collision)
        {
            base.OnTriggerEnter(collision);

            Destroy(gameObject, 0.5f);
            if(destroyFx)
            {
                var inst = Instantiate(gameObject);
                inst.transform.position = transform.position;
                inst.transform.rotation = transform.rotation;
                Destroy(inst, 3.0f);
            }
        }
    }
}