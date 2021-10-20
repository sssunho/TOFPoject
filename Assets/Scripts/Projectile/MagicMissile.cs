using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class MagicMissile : Projectile
    {
        public float expansionSpeed = 1f;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Update()
        {
            base.Update();
            if(!running)
            {
                transform.localScale += Time.deltaTime * expansionSpeed * Vector3.one;
            }
        }

        public void Shoot()
        {
            running = true;

            if (target != null)
            {
                transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position + Vector3.up);
            }

            velocity = Vector3.forward * speed;
            EnableDamageCollider();

            Destroy(gameObject, 3.0f);
        }
    }

}