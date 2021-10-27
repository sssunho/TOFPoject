using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class AttackManager
    {
        public const int maximumTargetNumber = 5;
        public static LayerMask CheckLayer = 1 << 9 | 1 << 11;

        public static void DamageToCone(Damage damage, Vector3 senderForward, Vector3 center, float radius, float angle)
        {
            Collider[] colliders = new Collider[maximumTargetNumber];
            Physics.OverlapSphereNonAlloc(center, radius, colliders, CheckLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] == null) return;

                Vector3 rel = colliders[i].transform.position - center;

                if (Vector3.Angle(senderForward, rel) < angle)
                {
                    var stat = colliders[i].GetComponent<CharacterStats>();
                    if (stat.teamIDNumber == damage.teamID) continue;
                    if (stat == null) continue;
                    stat.TakeDamage(damage);
                }
            }
        }

        public static void DamageToSphere(Damage damage, Vector3 center, float radius)
        {
            Collider[] colliders = new Collider[maximumTargetNumber];
            Physics.OverlapSphereNonAlloc(center, radius, colliders, CheckLayer);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] == null) return;

                var stat = colliders[i].GetComponent<CharacterStats>();

                if (stat.teamIDNumber == damage.teamID) continue;
                if (stat == null) continue;
                stat.TakeDamage(damage);
            }
        }

    }
}