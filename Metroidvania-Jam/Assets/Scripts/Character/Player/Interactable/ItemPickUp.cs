using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class ItemPickUp : Interactable
    {
        public Item item;

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            // Pick up the item and add it to inventory
        }

        private void PickUpItem(PlayerManager player)
        {
            PlayerInventoryManager playerInventoryManager = player.GetComponent<PlayerInventoryManager>();
            PlayerAnimatorManager playerAnimatorManager = player.GetComponent<PlayerAnimatorManager>();

            playerAnimatorManager.PlayTargetActionAnimation("Pick Up Item", true, true);
            playerInventoryManager.AddItem(item);
        }
    }
}
