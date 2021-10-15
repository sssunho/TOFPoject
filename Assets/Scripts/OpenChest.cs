using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TOF
{
    public class OpenChest : Interactable
    {
        Animator animator;
        OpenChest openChest;

        public Transform playerStandingPosition;
        public GameObject itemSpawner;
        public WeaponItem itemInChest;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            openChest = GetComponent<OpenChest>();
        }

        public override void Interact(PlayerManager playerManager)
        {
            // #.1 Rotate our Player towards the chest
            Vector3 rotationDirection = transform.position - playerManager.transform.position;
            rotationDirection.y = 0;
            rotationDirection.Normalize();

            Quaternion tr = Quaternion.LookRotation(rotationDirection);
            Quaternion targetRotation = Quaternion.Slerp(playerManager.transform.rotation, tr, 300 * Time.deltaTime);
            playerManager.transform.rotation = targetRotation;

            // #.2 Lock his transform to a certain point infront of the chest
            playerManager.OpenChestInteraction(playerStandingPosition);

            // #.3 open the chest lid, and animate the player
            animator.Play("Chest Open");

            // #.4 spawn an item inside the chest the player can pick up
            StartCoroutine(SpawnItemInChest());
            WeaponPickup weaponPickup = itemSpawner.GetComponent<WeaponPickup>();
            if(weaponPickup != null)
            {
                weaponPickup.weapon = itemInChest;
            }
        }

        private IEnumerator SpawnItemInChest()
        {
            yield return new WaitForSeconds(1f);
            Instantiate(itemSpawner, transform);
            Destroy(openChest);
        }
    }
}

