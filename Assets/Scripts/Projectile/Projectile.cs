using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class Projectile : DamageCollider
    {
        public GameObject destroyFx;
        public GameObject owner;
        public Vector3 velocity;
        public float angularSpeed = 10.0f;
        public bool chaseTarget = false;
        public GameObject target;
        public bool running;
        public float speed;

        protected override void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.gameObject.SetActive(true);
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        protected virtual void Update()
        {
            if (running)
            {
                if (chaseTarget)
                {
                    Quaternion look = Quaternion.LookRotation(transform.position - target.transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, look, angularSpeed * Time.deltaTime);
                }
                transform.Translate(velocity * Time.deltaTime);
            }
        }

        protected override void OnTriggerEnter(Collider collision)
        {
            if (collision.gameObject == owner)
                return;

            base.OnTriggerEnter(collision);
            damageCollider.enabled = false;
            Destroy(gameObject);
            if (destroyFx)
            {
                var inst = Instantiate(destroyFx);
                inst.transform.position = transform.position;
                inst.transform.rotation = transform.rotation;
                Destroy(inst, 3.0f);
            }
        }
    }
}