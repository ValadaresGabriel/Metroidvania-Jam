using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class PlayerLocomotionManager : CharacterLocomotionManager
    {
        private PlayerManager player;

        [HideInInspector] public float horizontalMovement;
        [HideInInspector] public float verticalMovement;
        [HideInInspector] public float moveAmount;

        [Header("Movement Settings")]
        [SerializeField] private float walkingSpeed = 2f;
        [SerializeField] private float runningSpeed = 5f;
        [SerializeField] private float sprintingSpeed = 6.5f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private int sprintingStaminaCost = 2;
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;

        [Header("Jump")]
        [SerializeField] private float jumpForwardSpeed = 5f;
        [SerializeField] private float freeFallSpeed = 2f;
        [SerializeField] private float jumpHeight = 4f;
        [SerializeField] private float jumpStaminaCost = 10f;
        private Vector3 jumpDirection;

        [Header("Dodge")]
        [SerializeField] private float dodgeStaminaCost = 25;
        private Vector3 rollDirection;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleJumpingMovement();
            HandleRotation();
            HandleFreeFallMovement();
        }

        private void GetMovementValues()
        {
            verticalMovement = PlayerInputManager.Instance.verticalInput;
            horizontalMovement = PlayerInputManager.Instance.horizontalInput;
            moveAmount = PlayerInputManager.Instance.moveAmount;

            // Clamp Movements
        }

        private void HandleGroundedMovement()
        {
            if (!player.canMove) return;

            GetMovementValues();

            moveDirection = PlayerCamera.Instance.transform.forward * verticalMovement;
            moveDirection += PlayerCamera.Instance.transform.right * horizontalMovement;
            moveDirection.Normalize();
            moveDirection.y = 0;

            if (player.isSprinting)
            {
                player.characterController.Move(moveDirection * sprintingSpeed * Time.deltaTime);
            }
            else
            {
                if (PlayerInputManager.Instance.moveAmount > 0.5f)
                {
                    // Running speed
                    player.characterController.Move(moveDirection * runningSpeed * Time.deltaTime);
                }
                else if (PlayerInputManager.Instance.moveAmount <= 0.5f)
                {
                    // Walking speed
                    player.characterController.Move(moveDirection * walkingSpeed * Time.deltaTime);
                }
            }
        }

        private void HandleJumpingMovement()
        {
            if (player.isJumping)
            {
                player.characterController.Move(jumpForwardSpeed * Time.deltaTime * jumpDirection);
            }
        }

        private void HandleFreeFallMovement()
        {
            if (!player.isGrounded)
            {
                Vector3 freeFallDirection;

                freeFallDirection = PlayerCamera.Instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
                freeFallDirection += PlayerCamera.Instance.cameraObject.transform.right * PlayerInputManager.Instance.horizontalInput;
                freeFallDirection.y = 0;

                player.characterController.Move(freeFallSpeed * Time.deltaTime * freeFallDirection);
            }
        }

        public void HandleRotation()
        {
            if (player.isDead) return;

            if (!player.canRotate) return;

            if (player.isLockedOn)
            {
                if (player.isSprinting || player.playerLocomotionManager.isRolling)
                {
                    Vector3 targetDirection = Vector3.zero;
                    targetDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
                    targetDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;
                    targetDirection.Normalize();
                    targetDirection.y = 0;

                    if (targetDirection == Vector3.zero)
                    {
                        targetDirection = transform.forward;
                    }

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
                else
                {
                    if (player.playerCombatManager.currentTarget == null) return;

                    Vector3 targetDirection;
                    targetDirection = player.playerCombatManager.currentTarget.transform.position - transform.position;
                    targetDirection.y = 0;
                    targetDirection.Normalize();

                    Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                    Quaternion finalRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    transform.rotation = finalRotation;
                }
            }
            else
            {
                targetRotationDirection = Vector3.zero;
                targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
                targetRotationDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;

                targetRotationDirection.Normalize();
                targetRotationDirection.y = 0;

                if (targetRotationDirection == Vector3.zero)
                {
                    targetRotationDirection = transform.forward;
                }

                Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
                Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

                transform.rotation = targetRotation;
            }
        }

        public void HandleSprinting()
        {
            if (player.isPerformingAction)
            {
                // Set sprinting do false
                player.isSprinting = false;
            }

            // If we are out of stamina, set sprinting to false
            if (player.characterStatsManager.GetCurrentStamina() <= 0)
            {
                player.isSprinting = false;
                return;
            }

            // If we are moving enough, sprinting is true, otherwise is false
            if (moveAmount > 0.5f)
            {
                player.isSprinting = true;
            }
            // If we are stationary, set sprinting to false
            else
            {
                player.isSprinting = false;
            }

            if (player.isSprinting)
            {
                player.characterStatsManager.SetCurrentStamina(player.characterStatsManager.GetCurrentStamina() - sprintingStaminaCost * Time.deltaTime);
            }
        }

        public void AttemptToPerformDodge()
        {
            if (player.isPerformingAction) return;

            // if (player.characterStatsManager.GetCurrentStamina() <= 0) return;

            // If we are moving, we perform a roll
            if (PlayerInputManager.Instance.moveAmount > 0)
            {
                rollDirection = PlayerCamera.Instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
                rollDirection += PlayerCamera.Instance.cameraObject.transform.right * PlayerInputManager.Instance.horizontalInput;
                rollDirection.y = 0;
                rollDirection.Normalize();

                Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
                player.transform.rotation = playerRotation;

                // Perform roll animation
                player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true);
                player.playerLocomotionManager.isRolling = true;
            }
            // If we are stationary, we perform a backstep
            else
            {
                // Perform backstep animation
                player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true);
            }

            player.characterStatsManager.SetCurrentStamina(player.characterStatsManager.GetCurrentStamina() - dodgeStaminaCost);
        }

        public void OpenDodge()
        {
            ChangeLayerRecursively(gameObject, 9);
        }

        public void CloseDodge()
        {
            ChangeLayerRecursively(gameObject, 6);
        }

        public void ChangeLayerRecursively(GameObject obj, int newLayer)
        {
            obj.layer = newLayer;

            foreach (Transform child in obj.transform)
            {
                ChangeLayerRecursively(child.gameObject, newLayer);
            }
        }

        public void AttemptToPerformJump()
        {
            if (player.isPerformingAction) return;

            if (player.isJumping) return;

            if (player.isGrounded == false) return;

            // if (player.characterStatsManager.GetCurrentStamina() <= 0) return;

            // If we are two handing our weapon, perform two handed jump animation, otherwise play the one handed animation
            player.playerAnimatorManager.PlayTargetActionAnimation("Th_Jump_Start_01", false);

            player.isJumping = true;

            // player.characterStatsManager.SetCurrentStamina(player.characterStatsManager.GetCurrentStamina() - jumpStaminaCost);
        }

        public void ApplyJumpingVelocity()
        {
            // Apply and upward velocity
            yVelocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityForce);
        }
    }
}
