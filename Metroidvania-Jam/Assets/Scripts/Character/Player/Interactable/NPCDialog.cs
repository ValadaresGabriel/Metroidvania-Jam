using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class NPCDialog : Interactable
    {
        [Header("NPC Informations")]
        [SerializeField] private int npcID;
        [SerializeField] private bool hasBeenMet;

        [Header("Dialog")]
        [SerializeField] private Dialog dialog;
        [SerializeField] private Dialog hasBeenMetDialog;

        protected override void Start()
        {
            base.Start();

            interactableText = "Press <b><size=40>E</size></b> or <b><size=40>Y</size></b> to interact";

            if (!WorldSaveGameManager.Instance.currentCharacterData.npcsInWorld.ContainsKey(npcID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.npcsInWorld.Add(npcID, false); // false = not looted
            }

            hasBeenMet = WorldSaveGameManager.Instance.currentCharacterData.npcsInWorld[npcID];
        }

        public override void Interact(PlayerManager playerManager)
        {
            base.Interact(playerManager);

            // Notify the character data this item has been looted from the world, so it doesn't spawn again
            if (WorldSaveGameManager.Instance.currentCharacterData.npcsInWorld.ContainsKey(npcID))
            {
                WorldSaveGameManager.Instance.currentCharacterData.npcsInWorld.Remove(npcID);
            }

            WorldSaveGameManager.Instance.currentCharacterData.npcsInWorld.Add(npcID, true);

            hasBeenMet = true;

            StartDialog();
        }

        private void StartDialog()
        {
            if (hasBeenMet)
            {
                PlayerUIManager.Instance.playerUIDialogManager.InitializeDialog(hasBeenMetDialog);
                return;
            }

            PlayerUIManager.Instance.playerUIDialogManager.InitializeDialog(dialog);
        }
    }
}
