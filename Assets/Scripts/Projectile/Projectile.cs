using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class Projectile : DamageCollider
    {
        public GameObject destroyFx;
        public GameObject owner;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = true;
        }

        private void Update()
        {
            //transform.Translate(Vector3.forward * Time.deltaTime);
        }

        protected override void OnTriggerEnter(Collider collision)
        {
            base.OnTriggerEnter(collision);

            damageCollider.enabled = false;
            Destroy(gameObject);
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