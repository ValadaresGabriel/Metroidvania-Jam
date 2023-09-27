using System;
using UnityEngine;
using TMPro;

namespace TS
{
    public class PlayerUIDialogManager : MonoBehaviour
    {
        [SerializeField] private GameObject dialogGameObject;
        [SerializeField] private TextMeshProUGUI dialogOwnerText;
        [SerializeField] private TextMeshProUGUI dialogText;

        private Dialog currentDialog;
        private PlayerUIDialogInputManager dialogInputManager;
        private int currentDialogIndex = 0;

        public void InitializeDialog(Dialog dialog)
        {
            SetCurrentDialog(dialog);

            dialogGameObject.SetActive(true);

            dialogInputManager = dialogGameObject.GetComponent<PlayerUIDialogInputManager>();
            dialogInputManager.ContinueToNextDialogEvent += ContinueToNextDialog;

            dialogOwnerText.SetText(currentDialog.owner);
            dialogText.SetText(currentDialog.text);
        }

        public void ContinueToNextDialog()
        {
            currentDialogIndex++;

            if (currentDialogIndex < currentDialog.DialogSequence.Count)
            {
                dialogOwnerText.SetText(currentDialog.DialogSequence[currentDialogIndex].owner);
                dialogText.SetText(currentDialog.DialogSequence[currentDialogIndex].text);
                return;
            }

            CloseDialog();
        }

        private void CloseDialog()
        {
            currentDialogIndex = 0;
            dialogInputManager.ContinueToNextDialogEvent -= ContinueToNextDialog;
            PlayerInputManager.Instance.IsInteracting = false;
            dialogGameObject.SetActive(false);
        }

        private void SetCurrentDialog(Dialog newDialog)
        {
            currentDialog = newDialog;
        }
    }
}
