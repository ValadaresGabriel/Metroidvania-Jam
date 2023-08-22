using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IM
{
    public class PlayerInputManager : MonoBehaviour
    {
        public static PlayerInputManager Instance { get; private set; }

        private PlayerControls playerControls;

        [HideInInspector]
        public PlayerManager player;

        [Header("CAMERA MOVEMENT INPUT")]

        [SerializeField]
        private Vector2 cameraInput;

        public float cameraHorizontalInput;

        public float cameraVerticalInput;

        [Header("PLAYER MOVEMENT INPUT")]
        public Vector2 movementInput;

        public float horizontalInput;

        public float verticalInput;

        public float moveAmount;

        [Header("PLAYER ACTION INPUT")]

        [SerializeField]
        private bool dodgeInput = false;

        [SerializeField]
        private bool sprintInput = false;

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
        }

        private void OnEnable()
        {
            if (playerControls == null)
            {
                playerControls = new PlayerControls();

                playerControls.PlayerLocomotion.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
                playerControls.PlayerActions.Dodge.performed += i => dodgeInput = true;

                // Holding the input
                playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
                playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;
            }

            playerControls.Enable();
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            if (newScene.buildIndex == GameManager.Instance.GetWorldSceneIndex())
            {
                Instance.enabled = true;
            }
            else
            {
                Instance.enabled = false;
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
            HandleSprinting();
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

            // If we are not locked on, only use the move amount
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);

            // If we are locked on pass the horizontal movement as well
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

        private void HandleSprinting()
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
        #endregion

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
