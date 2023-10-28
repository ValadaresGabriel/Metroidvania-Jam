using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class PlayerPowerUpPickUp : Interactable
    {
        [Header("Item Informations")]
        [SerializeField] private int powerUpID;
        [SerializeField] private bool hasBeenLooted;
        [SerializeField] private bool canUseHeavyAttack;

        [Header("Item")]
        public Item item;

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void Start()
        {
            base.Start();

            interactableText = " to interact";

            if (!WorldSaveGameManager.Instance.currentCharacterData.powerUpsInWorld.ContainsKey(powerUpID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.powerUpsInWorld.Add(powerUpID, false); // false = not looted
            }

            hasBeenLooted = WorldSaveGameManager.Instance.currentCharacterData.powerUpsInWorld[powerUpID];

            if (hasBeenLooted)
            {
                gameObject.SetActive(false);
            }
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            // Notify the character data this item has been looted from the world, so it doesn't spawn again
            if (WorldSaveGameManager.Instance.currentCharacterData.powerUpsInWorld.ContainsKey(powerUpID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.powerUpsInWorld.Remove(powerUpID);
            }

            WorldSaveGameManager.Instance.currentCharacterData.powerUpsInWorld.Add(powerUpID, true);

            hasBeenLooted = true;

            // Pick up the item and add it to inventory
            PickUpItem(playerManager);
        }

        private void PickUpItem(PlayerManager player)
        {
            player.canUseHeavyAttack = canUseHeavyAttack;

            if (canUseHeavyAttack)
                PlayerUIManager.Instance.playerUIPopupManager.InitializeInteractResponse(item);

            gameObject.SetActive(false);
        }
    }
}
