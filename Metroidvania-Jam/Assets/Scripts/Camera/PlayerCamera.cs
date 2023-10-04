using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using System.Collections.Generic;
using System;

namespace TS
{
    public enum LockOnRightLeftTarget
    {
        Right,
        Left
    }

    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }

        [HideInInspector] public PlayerManager player;
        [SerializeField] public Camera cameraObject;
        [SerializeField] private Transform cameraPivotTransform;

        [Header("Camera Settings")]
        private float cameraSmoothSpeed = 1; // The bigger this number is, the longer for the camera to reach its position during movement
        [SerializeField] private float leftAndRightRotationSpeed = 220f;
        [SerializeField] private float upAndDownRotationSpeed = 220f;
        [SerializeField] private float minimumPivot = -30f; // The lowest point you are able to look down
        [SerializeField] private float maximumPivot = 60f; // The highest point you are able to look down
        [SerializeField] private float cameraCollisionRadius = 0.2f; // The highest point you are able to look down
        [SerializeField] private LayerMask collideWithLayers;
        [SerializeField] private LayerMask environmentLayer;

        [Header("Camera Values")]
        private Vector3 cameraVelocity;
        private Vector3 cameraObjectPosition; // Used for camera collisions (moves the camera object to this position)
        [SerializeField] private float leftAndRightLookAngle;
        [SerializeField] private float upAndDownLookAngle;

        private float cameraZPosition; // Value user for camera collision
        private float targetCameraZPosition; // Value user for camera collision

        [Header("Lock On")]
        [SerializeField] private float lockOnRadius = 20f;
        [SerializeField] private float minimumViewableAngle = -50f;
        [SerializeField] private float maximumViewableAngle = 50f;
        [SerializeField] private float maximumLockOnDistance = 20f;

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
            cameraZPosition = cameraObject.transform.localPosition.z;
        }

        public void HandleAllCameraActions()
        {
            if (player != null)
            {
                HandleFollowTarget();
                HandleRotations();
                HandleCollisions();
            }
        }

        private void HandleFollowTarget()
        {
            Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed * Time.deltaTime);
            transform.position = targetCameraPosition;
        }

        private void HandleRotations()
        {
            // If the cursor is visible, don't rotate the camera
            if (WorldCursorManager.Instance != null && WorldCursorManager.Instance.IsCursorVisible) return;

            // If locked on, force rotation towards the target
            // else rotate regularly
            Quaternion targetRotation;

            #region Normal Camera Rotation
            // Normal rotations
            // Rotate left and right based on horizontal movement on the right joystick
            leftAndRightLookAngle += PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed * Time.deltaTime;
            // Rotate up and down based on vertical movement on the right joystick
            upAndDownLookAngle -= PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed * Time.deltaTime;

            // Clamp the up and down look angle between a min and max value
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation;
            # region Rotate this GameObject left and right
            cameraRotation = Vector3.zero;

            cameraRotation.y = leftAndRightLookAngle;

            targetRotation = Quaternion.Euler(cameraRotation);
            transform.rotation = targetRotation;
            #endregion

            #region Rotate this GameObject up and down
            cameraRotation = Vector3.zero;

            cameraRotation.x = upAndDownLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);

            cameraPivotTransform.localRotation = targetRotation;
            #endregion

            #endregion
        }

        private void HandleCollisions()
        {
            targetCameraZPosition = cameraZPosition;

            RaycastHit hit;
            Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
            direction.Normalize();

            if (Physics.SphereCast(cameraPivotTransform.position, cameraCollisionRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers))
            {
                float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);

                targetCameraZPosition = -(distanceFromHitObject - cameraCollisionRadius);
            }

            if (Mathf.Abs(targetCameraZPosition) < cameraCollisionRadius)
            {
                targetCameraZPosition = -cameraCollisionRadius;
            }

            cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);

            cameraObject.transform.localPosition = cameraObjectPosition;
        }

        public void HandleLocatingLockOnTargets()
        {
            float shortDistance = Mathf.Infinity;
            float shortDistanceOfRightTarget = Mathf.Infinity;
            float shortDistanceOfLeftTarget = -Mathf.Infinity;

            // TO DO: use layer mask
            Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.Instance.GetCharacterLayers);

            foreach (var target in colliders)
            {
                if (target.TryGetComponent(out CharacterManager lockOnTarget))
                {
                    Vector3 lockOnTargetsDirection = lockOnTarget.transform.position - player.transform.position;
                    float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                    float viewableAngle = Vector3.Angle(lockOnTargetsDirection, cameraObject.transform.forward);

                    if (lockOnTarget.isDead)
                        continue;

                    // If is player, check for next potential target
                    if (lockOnTarget.transform.root == player.transform.root)
                        continue;

                    if (distanceFromTarget > maximumLockOnDistance)
                        continue;

                    if (viewableAngle > minimumViewableAngle && viewableAngle < maximumViewableAngle)
                    {
                        RaycastHit hit;

                        // TO DO: add layer for enviro layers only
                        if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position, lockOnTarget.characterCombatManager.lockOnTransform.position, out hit, WorldUtilityManager.Instance.GetEnviroLayers))
                        {
                            // We hit something, we cannot see our lock on target
                            continue;
                        }
                        else
                        {
                            Debug.Log("Target found!");
                        }
                    }
                }
            }
        }

        // public void SetCameraHeight()
        // {
        //     Vector3 velocity = Vector3.zero;
        //     Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
        //     Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

        //     if (lockOnCurrentTarget != null)
        //     {
        //         cameraPivotTransform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
        //     }
        //     else
        //     {
        //         cameraPivotTransform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
        //     }
        // }

        // public Transform GetLockOnCurrentTarget() => lockOnCurrentTarget.transform;
    }
}