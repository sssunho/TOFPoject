using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class Boss1MagicManager : MonoBehaviour
    {
        public GameObject MagicBomb;
        public GameObject MagicArrow;
        public GameObject MagicBall;

        public void PutMagicBomb()
        {
            var inst = Instantiate(MagicBomb);
            inst.transform.parent = null;
            inst.transform.position = transform.position;
            inst.transform.rotation = transform.rotation;
        }
    }
}
