using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TS
{
    public class PlayerUIInteractMessage : MonoBehaviour
    {
        [SerializeField] private GameObject interactMessageGameObject;
        [SerializeField] private TextMeshProUGUI interactMessage;

        public void InitializeInteractMessage(string newText)
        {
            interactMessageGameObject.SetActive(true);
            SetInteractMessage(newText);
        }

        public void SetInteractMessage(string newText)
        {
            interactMessage.SetText(newText);
        }

        public void CloseInteractMessage()
        {
            interactMessageGameObject.SetActive(false);
        }
    }
}
