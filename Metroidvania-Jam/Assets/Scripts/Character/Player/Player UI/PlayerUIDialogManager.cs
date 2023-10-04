using System;
using UnityEngine;
using TMPro;
using System.Collections;

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

        [Header("Typing Text Configurations")]
        [SerializeField]
        private float typeSpeed = 0.01f;  // Velocidade que o texto aparece
        private bool isFastForwarding = false;  // Controle para exibir todo o texto
        private bool isDialogueCompleted = false; // Controle para saber se o texto foi completamente exibido
        private string currentDialogueText = "";
        private float timer = 0f;
        private int charIndex = 0;

        private void Update()
        {
            if (!isDialogueCompleted)
            {
                AnimateText();
            }
        }

        public void InitializeDialog(Dialog dialog)
        {
            SetCurrentDialog(dialog);

            dialogGameObject.SetActive(true);

            dialogInputManager = dialogGameObject.GetComponent<PlayerUIDialogInputManager>();
            dialogInputManager.ContinueToNextDialogEvent += ContinueToNextDialog;

            dialogOwnerText.SetText(currentDialog.owner);
            SetDialogueText(dialog.text);

            // dialogText.SetText(currentDialog.text);
        }

        private void SetDialogueText(string newDialogue)
        {
            currentDialogueText = newDialogue;
            dialogText.SetText("");
            charIndex = 0;
            timer = 0f;
            isDialogueCompleted = false;
            isFastForwarding = false;
        }

        private void AnimateText()
        {
            timer += Time.deltaTime;

            if (timer >= typeSpeed)
            {
                if (charIndex < currentDialogueText.Length)
                {
                    dialogText.text += currentDialogueText[charIndex];
                    charIndex++;
                }
                else
                {
                    isDialogueCompleted = true;
                }

                timer = 0f;
            }

            if (isFastForwarding)
            {
                dialogText.SetText(currentDialogueText);
                isDialogueCompleted = true;
                isFastForwarding = false;
            }
        }

        public void ContinueToNextDialog()
        {
            if (!isDialogueCompleted)
            {
                isFastForwarding = true;
                return;
            }

            currentDialogIndex++;

            if (currentDialogIndex < currentDialog.DialogSequence.Count)
            {
                dialogOwnerText.SetText(currentDialog.DialogSequence[currentDialogIndex].owner);
                SetDialogueText(currentDialog.DialogSequence[currentDialogIndex].text);
                return;
            }

            CloseDialog();
        }

        // public void ContinueToNextDialog()
        // {
        //     currentDialogIndex++;

        //     if (currentDialogIndex < currentDialog.DialogSequence.Count)
        //     {
        //         dialogOwnerText.SetText(currentDialog.DialogSequence[currentDialogIndex].owner);
        //         dialogText.SetText(currentDialog.DialogSequence[currentDialogIndex].text);
        //         return;
        //     }

        //     CloseDialog();
        // }

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
