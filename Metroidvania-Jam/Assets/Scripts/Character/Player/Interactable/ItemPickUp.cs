using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class ItemPickUp : Interactable
    {
        [Header("Item Informations")]
        [SerializeField] private int itemPickUpID;
        [SerializeField] private bool hasBeenLooted;

        [Header("Item")]
        public Item item;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            if (!WorldSaveGameManager.Instance.currentCharacterData.itemsInWorld.ContainsKey(itemPickUpID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.itemsInWorld.Add(itemPickUpID, false); // false = not looted
            }

            hasBeenLooted = WorldSaveGameManager.Instance.currentCharacterData.itemsInWorld[itemPickUpID];

            if (hasBeenLooted)
            {
                gameObject.SetActive(false);
            }
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            // Notify the character data this item has been looted from the world, so it doesn't spawn again
            if (WorldSaveGameManager.Instance.currentCharacterData.itemsInWorld.ContainsKey(itemPickUpID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.itemsInWorld.Remove(itemPickUpID);
            }

            Debug.Log(WorldSaveGameManager.Instance.currentCharacterData.itemsInWorld.Count);

            WorldSaveGameManager.Instance.currentCharacterData.itemsInWorld.Add(itemPickUpID, true);

            Debug.Log(WorldSaveGameManager.Instance.currentCharacterData.itemsInWorld.Count);
            Debug.Log(WorldSaveGameManager.Instance.currentCharacterData.itemsInWorld[itemPickUpID]);

            hasBeenLooted = true;

            // Pick up the item and add it to inventory
            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager player)
        {
            PlayerInventoryManager playerInventoryManager = player.GetComponent<PlayerInventoryManager>();
            PlayerAnimatorManager playerAnimatorManager = player.GetComponent<PlayerAnimatorManager>();

            // playerAnimatorManager.PlayTargetActionAnimation("Pick Up Item", true, true);
            playerInventoryManager.AddItem(item);

            gameObject.SetActive(false);
        }
    }
}
