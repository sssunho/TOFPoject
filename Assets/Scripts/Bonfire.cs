using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class Bonfire : Interactable
    {
        public GameObject gameObject;
        public bool isIgnited = false;

        public override void Interact(PlayerManager playerManager)
        {
            playerManager.BonFireInteraction(isIgnited);
            if (!isIgnited)
            {
                isIgnited = true;
                Invoke("Firing", 3f);
            }
        }

        public void Firing()
        {
            gameObject.SetActive(true);
        }
    }
}

