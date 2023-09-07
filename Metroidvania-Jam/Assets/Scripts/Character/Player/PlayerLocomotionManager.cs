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

        [Header("MOVEMENT SETTINGS")]
        private Vector3 moveDirection;
        private Vector3 targetRotationDirection;
        [SerializeField] private float walkingSpeed = 2f;
        [SerializeField] private float runningSpeed = 5f;
        [SerializeField] private float sprintingSpeed = 6.5f;
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private int sprintingStaminaCost = 2;

        [Header("DODGE")]
        private Vector3 rollDirection;
        [SerializeField] private float dodgeStaminaCost = 25;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        protected override void Update()
        {
            base.Update();

            // player.characterNetworkManager.verticalMovement = verticalMovement;
            // player.characterNetworkManager.horizontalMovement = horizontalMovement;
            // player.characterNetworkManager.moveAmount = moveAmount;
        }

        public void HandleAllMovement()
        {
            HandleGroundedMovement();
            HandleRotation();
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

        private void HandleRotation()
        {
            if (!player.canRotate) return;

            targetRotationDirection = Vector3.zero;
            targetRotationDirection = PlayerCamera.Instance.cameraObject.transform.forward * verticalMovement;
            targetRotationDirection += PlayerCamera.Instance.cameraObject.transform.right * horizontalMovement;

            targetRotationDirection.Normalize();
            targetRotationDirection.y = 0;

            if (player.isLockedOnEnemy)
            {
                // TO DO: isDodging
                if (player.isSprinting || player.isSprinting)
                {
                    if (targetRotationDirection == Vector3.zero)
                    {
                        targetRotationDirection = transform.forward;
                    }

                    Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
                else
                {
                    targetRotationDirection = moveDirection;
                    targetRotationDirection = PlayerCamera.Instance.GetLockOnCurrentTarget().position - transform.position;
                    targetRotationDirection.Normalize();
                    targetRotationDirection.y = 0;

                    Quaternion newRotation = Quaternion.LookRotation(targetRotationDirection);
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, newRotation, rotationSpeed * Time.deltaTime);

                    transform.rotation = targetRotation;
                }
            }
            else
            {
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
            }
            // If we are stationary, we perform a backstep
            else
            {
                // Perform backstep animation
                player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true);
            }

            player.characterStatsManager.SetCurrentStamina(player.characterStatsManager.GetCurrentStamina() - dodgeStaminaCost);
        }
    }
}


// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.InputSystem;

// namespace TS
// {
//     public class PlayerLocomotionManager : CharacterLocomotionManager
//     {
//         private PlayerManager player;

//         [HideInInspector]
//         public float horizontalMovement;

//         [HideInInspector]
//         public float verticalMovement;

//         [HideInInspector]
//         public float moveAmount;

//         [Header("MOVEMENT SETTINGS")]

//         [SerializeField]
//         private float turnSpeed;

//         [SerializeField]
//         private float walkingSpeed = 2f;

//         [SerializeField]
//         private float runningSpeed = 5f;

//         [SerializeField]
//         private float sprintingSpeed = 6.5f;

//         [SerializeField]
//         private float rotationSpeed = 15f;

//         [SerializeField]
//         private int sprintingStaminaCost = 2;

//         [Header("DODGE")]
//         private Vector3 rollDirection;

//         [SerializeField]
//         private float dodgeStaminaCost = 25;

//         protected override void Awake()
//         {
//             base.Awake();

//             player = GetComponent<PlayerManager>();
//         }

//         protected override void Update()
//         {
//             base.Update();

//             // player.characterNetworkManager.verticalMovement = verticalMovement;
//             // player.characterNetworkManager.horizontalMovement = horizontalMovement;
//             // player.characterNetworkManager.moveAmount = moveAmount;
//         }

//         public void HandleAllMovement()
//         {
//             HandleGroundedMovement();
//             // HandleRotation();
//         }

//         private void GetMovementValues()
//         {
//             verticalMovement = PlayerInputManager.Instance.verticalInput;
//             horizontalMovement = PlayerInputManager.Instance.horizontalInput;
//             moveAmount = PlayerInputManager.Instance.moveAmount;
//         }

//         private void HandleGroundedMovement()
//         {
//             if (!player.canMove) return;

//             GetMovementValues();

//             Vector3 input = new Vector3(horizontalMovement, 0, verticalMovement).ToIso();

//             if (input.magnitude > 0)
//             {
//                 float speed = walkingSpeed;

//                 if (player.isSprinting)
//                     speed = sprintingSpeed;
//                 else if (PlayerInputManager.Instance.moveAmount > 0.5f)
//                     speed = runningSpeed;

//                 player.RB.MovePosition(transform.position + input * speed * Time.deltaTime);
//             }

//             HandleRotation();
//         }

//         private void HandleRotation()
//         {
//             if (PlayerInputManager.Instance.movementInput == Vector2.zero) return;

//             Vector3 input = new Vector3(horizontalMovement, 0, verticalMovement).ToIso();
//             var rot = Quaternion.LookRotation(input, Vector3.up);
//             transform.rotation = Quaternion.RotateTowards(transform.rotation, rot, turnSpeed * Time.deltaTime);
//         }

//         public void HandleSprinting()
//         {
//             if (player.isPerformingAction)
//             {
//                 // Set sprinting do false
//                 player.isSprinting = false;
//             }

//             // If we are out of stamina, set sprinting to false
//             if (player.characterStatsManager.GetCurrentStamina() <= 0)
//             {
//                 player.isSprinting = false;
//                 return;
//             }

//             // If we are moving enough, sprinting is true, otherwise is false
//             if (moveAmount > 0.5f)
//             {
//                 player.isSprinting = true;
//             }
//             // If we are stationary, set sprinting to false
//             else
//             {
//                 player.isSprinting = false;
//             }

//             if (player.isSprinting)
//             {
//                 player.characterStatsManager.SetCurrentStamina(player.characterStatsManager.GetCurrentStamina() - sprintingStaminaCost * Time.deltaTime);
//             }
//         }

//         // public void AttemptToPerformDodge()
//         // {
//         //     if (player.isPerformingAction) return;

//         //     if (player.characterStatsManager.GetCurrentStamina() <= 0) return;

//         //     // If we are moving, we perform a roll
//         //     if (PlayerInputManager.Instance.moveAmount > 0)
//         //     {
//         //         rollDirection = PlayerCamera.Instance.cameraObject.transform.forward * PlayerInputManager.Instance.verticalInput;
//         //         rollDirection += PlayerCamera.Instance.cameraObject.transform.right * PlayerInputManager.Instance.horizontalInput;
//         //         rollDirection.y = 0;
//         //         rollDirection.Normalize();

//         //         Quaternion playerRotation = Quaternion.LookRotation(rollDirection);
//         //         player.transform.rotation = playerRotation;

//         //         // Perform roll animation
//         //         player.playerAnimatorManager.PlayTargetActionAnimation("Roll_Forward_01", true);
//         //     }
//         //     // If we are stationary, we perform a backstep
//         //     else
//         //     {
//         //         // Perform backstep animation
//         //         player.playerAnimatorManager.PlayTargetActionAnimation("Back_Step_01", true);
//         //     }

//         //     player.characterStatsManager.GetCurrentStamina() -= dodgeStaminaCost;
//         // }
//     }

//     public static class Helpers
//     {
//         private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
//         public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint3x4(input);
//     }
// }
