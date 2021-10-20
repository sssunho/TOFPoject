using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class PassFogWall : Interactable
    {
        FogWall fogWall;
        CameraHandler cameraHandler;

        private void Awake()
        {
            fogWall = GetComponentInParent<FogWall>();
            cameraHandler = FindObjectOfType<CameraHandler>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);
            playerManager.PassThroughFogWallInteraction(transform);
            cameraHandler.PassingShot();
            fogWall.PassFog();
        }
    }
}