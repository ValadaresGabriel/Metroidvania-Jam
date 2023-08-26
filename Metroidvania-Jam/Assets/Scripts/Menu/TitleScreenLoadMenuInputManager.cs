using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class TitleScreenLoadMenuInputManager : MonoBehaviour
    {
        private PlayerControls playerControls;

        [Header("Title Screen Inputs")]
        public bool deleteCharacterSlot = false;

        private void Update()
        {
            if (deleteCharacterSlot)
            {
                deleteCharacterSlot = false;
                TitleScreenManager.Instance.AttemptToDeleteCharacterSlot();
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();
                playerControls.UI.X.performed += i => deleteCharacterSlot = true;
            }

            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }
    }
}
