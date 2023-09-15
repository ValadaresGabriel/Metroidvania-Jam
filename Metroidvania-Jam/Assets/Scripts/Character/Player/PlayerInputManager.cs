using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TS
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance { get; private set; }

        private PlayerControls playerControls;

        [HideInInspector] public PlayerManager player;

        [Header("CAMERA MOVEMENT INPUT")]
        [SerializeField] private Vector2 cameraInput;
        public float cameraHorizontalInput;
        public float cameraVerticalInput;

        [Header("PLAYER MOVEMENT INPUT")]
        public Vector2 movementInput;
        public float horizontalInput;
        public float verticalInput;
        public float moveAmount;

        [Header("PLAYER ACTION INPUT")]
        [SerializeField] private bool A_Input = false;
        [SerializeField] private bool interactInput = false;
        [SerializeField] private bool dodgeInput = false;
        [SerializeField] private bool sprintInput = false;
        [SerializeField] private bool jumpInput = false;
        [SerializeField] private bool lockOnInput = false;
        [SerializeField] private bool rightStick_Right = false;
        [SerializeField] private bool rightStick_Left = false;

        private bool isMenuOpened = false;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);

            SceneManager.activeSceneChanged += OnSceneChange;

            Instance.enabled = false;

            if (playerControls != null)
            {
                playerControls.Disable();
            }
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerLocomotion.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();

                playerControls.PlayerActions.LightAttack.performed += i => A_Input = true;
                playerControls.PlayerActions.Interact.performed += i => interactInput = true;
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;

                // Jump
                playerControls.PlayerActions.Jump.performed += i => jumpInput = true;

                // Lock On
                playerControls.PlayerActions.LockOn.performed += i => lockOnInput = true;
                playerControls.PlayerCamera.LockOnTargetRight.performed += i => rightStick_Right = true;
                playerControls.PlayerCamera.LockOnTargetLeft.performed += i => rightStick_Left = true;

                // Holding the input
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

                // Pause
                playerControls.UI.Pause.performed += i => HandlePause();
            }

            playerControls.Enable();
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (WorldSaveGameManager.Instance.GetWorldSceneIndex().Contains(newScene.buildIndex))
            {
                Instance.enabled = true;

                if (playerControls != null)
                {
                    playerControls.Enable();
                }
            }
            else
            {
                Instance.enabled = false;

                if (playerControls != null)
                {
                    playerControls.Disable();
                }
            }
        }

        private void Update()
        {
            HandleAllInputs();
        }

        private void HandleAllInputs()
        {
            HandlePlayerMovementInput();
            HandleCameraMovementInput();
            HandleDodgeInput();
            HandleSprintingInput();
            HandleAInput();
            HandleLockOnInput();
            HandleJumpInput();

            AttemptToInteract();
        }

        #region Movement
        private void HandlePlayerMovementInput()
        {
            horizontalInput = movementInput.x;
            verticalInput = movementInput.y;

            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

            if (moveAmount <= 0.5f && moveAmount > 0)
            {
                moveAmount = 0.5f;
            }
            else if (moveAmount > 0.5f && moveAmount <= 1)
            {
                moveAmount = 1f;
            }

            if (player == null) return;

            // If we are locked on pass the horizontal movement as well
            if (player.isLockedOnEnemy && player.isSprinting == false)
            {
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput);
            }
            else
            {
                // If we are not locked on, only use the move amount
                player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);
            }
        }

        private void HandleCameraMovementInput()
        {
            cameraVerticalInput = cameraInput.y;
            cameraHorizontalInput = cameraInput.x;
        }
        #endregion

        #region Action
        private void HandleDodgeInput()
        {
            if (dodgeInput)
            {
                dodgeInput = false;

                // Future note: return (DO NOTHING) if menu or UI window is open, do nothing

                // Perform dodge
                player.playerLocomotionManager.AttemptToPerformDodge();
            }
        }

        private void HandleSprintingInput()
        {
            if (sprintInput)
            {
                player.playerLocomotionManager.HandleSprinting();
            }
            else
            {
                player.isSprinting = false;
            }
        }

        private void HandleJumpInput()
        {
            if (jumpInput)
            {
                jumpInput = false;

                // If UI is open, do nothing (return)
                player.playerLocomotionManager.AttemptToPerformJump();
            }
        }

        private void AttemptToInteract()
        {
            if (interactInput)
            {
                interactInput = false;
                player.interactableManager.AttemptToInteract();
            }
        }

        private void HandleAInput()
        {
            if (A_Input)
            {
                A_Input = false;

                player.playerCombatManager.PerformWeaponBasedAction(player.playerInventoryManager.currentRightHandWeapon.th_A_Action, player.playerInventoryManager.currentRightHandWeapon);
            }
        }

        private void HandleLockOnInput()
        {
            if (lockOnInput && player.isLockedOnEnemy == false)
            {
                lockOnInput = false;

                PlayerCamera.Instance.AttemptToLockOn();
            }
            else if (lockOnInput && player.isLockedOnEnemy)
            {
                lockOnInput = false;
                player.isLockedOnEnemy = false;

                PlayerCamera.Instance.ClearLockOnTargets();
            }

            if (player.isLockedOnEnemy && rightStick_Left)
            {
                rightStick_Left = false;
                PlayerCamera.Instance.AttemptToSetLockOnBasedOnLeftOrRightTarget(LockOnRightLeftTarget.Left);
            }

            if (player.isLockedOnEnemy && rightStick_Right)
            {
                rightStick_Right = false;
                PlayerCamera.Instance.AttemptToSetLockOnBasedOnLeftOrRightTarget(LockOnRightLeftTarget.Right);
            }
            PlayerCamera.Instance.SetCameraHeight();
        }
        #endregion

        private void HandlePause()
        {
            PlayerUIManager.Instace.playerUIPauseManager.InitializePause();
        }

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
