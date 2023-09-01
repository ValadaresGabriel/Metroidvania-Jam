using System;
using UnityEngine;

namespace TS
{
    public class PlayerUIDialogInputManager : MonoBehaviour
    {
        public event Action ContinueToNextDialogEvent;
        private PlayerControls playerControls;

        private void Awake()
        {
            InitializePlayerControls();
        }

        private void InitializePlayerControls()
        {
            playerControls = new PlayerControls();
            playerControls.UI.A.performed += HandleXButtonPerformed;
        }

        private void HandleXButtonPerformed(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            ContinueToNextDialogEvent?.Invoke();
        }

        private void OnEnable()
        {
            playerControls.Enable();
        }

        private void OnDisable()
        {
            playerControls.Disable();
        }

        private void OnDestroy()
        {
            // Clean up the event handler
            playerControls.UI.X.performed -= HandleXButtonPerformed;
        }
    }
}
