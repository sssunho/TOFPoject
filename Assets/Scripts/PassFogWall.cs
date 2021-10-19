using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PassFogWall : Interactable
    {
        FogWall fogWall;

        private void Awake()
        {
            fogWall = GetComponentInParent<FogWall>();
        }

        public override void Interact(PlayerManager playerManager)
        {

            base.Interact(playerManager);
            playerManager.PassThroughFogWallInteraction(transform);
            fogWall.DeactivateFogWall();
        }
    }
}

