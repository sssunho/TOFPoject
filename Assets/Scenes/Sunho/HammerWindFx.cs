using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class HammerWindFx : MonoBehaviour
    {
        public float preFxSpeed = 3.0f;
        public GameObject[] preFx;
        public GameObject[] afterFx;
        public CharacterStats target;

        float timer = 0.0f;
        float angle = 120;
        float r = 6.0f;
        bool active = false;

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer < 1.0f)
            {
                foreach (GameObject fx in preFx)
                    fx.transform.Translate(Vector3.forward * preFxSpeed * Time.deltaTime);
            }
            else if (timer > 2.0f && !active)
            {
                active = true;
                foreach (GameObject fx in preFx)
                    Destroy(fx);
                foreach (GameObject fx in afterFx)
                    fx.SetActive(true);

                if (isTargetInSector())
                {
                    var stat = target.GetComponent<PlayerStats>();
                    stat.TakeDamage(20);
                }

            }
            else if (timer > 3.0f)
            {
                Destroy(gameObject);
            }
        }

        bool isTargetInSector()
        {
            Vector3 rel = target.transform.position - transform.position;
            return rel.magnitude < r && Vector3.Angle(rel, transform.forward) < angle / 2.0f;
        }
    }
}