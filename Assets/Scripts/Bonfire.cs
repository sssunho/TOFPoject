using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class Bonfire : Interactable
    {
        public GameObject light;
        public bool isIgnited = false;
        public Transform checkPoint;

        public override void Interact(PlayerManager playerManager)
        {
            playerManager.BonFireInteraction(this);
            if (!isIgnited)
            {
                Invoke("Firing", 2f);
            }
        }

        public void Firing()
        {
            light.SetActive(true);
            isIgnited = true;
        }
    }
}

