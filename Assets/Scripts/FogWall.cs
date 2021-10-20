using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class FogWall : MonoBehaviour
    {
        public BoxCollider entrance;
        public BoxCollider wall;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void ActivateFogWall()
        {
            gameObject.SetActive(true);
        }

        public void DeactivateFogWall()
        {
            gameObject.SetActive(false);
        }

        public void PassFog()
        {
            entrance.gameObject.SetActive(false);
            wall.gameObject.SetActive(false);
        }

        public void FogPassed()
        {
            entrance.gameObject.SetActive(true);
            wall.gameObject.SetActive(true);
        }
    }
}

