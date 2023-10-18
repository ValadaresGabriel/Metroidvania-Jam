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
        [SerializeField] private float lockOnTargetFollowSpeed = 0.2f;
        [SerializeField] private float unlockedCameraHeight = 1.5f;
        [SerializeField] private float lockedCameraHeight = 2.0f;
        [SerializeField] private float setCameraHeightSpeed = 0.05f;
        private List<CharacterManager> avaliableTargets = new();
        private Coroutine cameraLockOnHeightCoroutine;
        public CharacterManager nearestLockOnTarget { get; private set; }
        public CharacterManager leftLockOnTarget { get; private set; }
        public CharacterManager rightLockOnTarget { get; private set; }

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

            // Lock on camera rotation
            if (player.isLockedOn)
            {
                // Rotates this gameobject
                Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
                rotationDirection.Normalize();
                rotationDirection.y = 0;
                Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // Rotates pivot object
                rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
                rotationDirection.Normalize();
                targetRotation = Quaternion.LookRotation(rotationDirection);
                cameraPivotTransform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

                // Save our rotations to our look angles, so when we unlock it doesnt snap too far away
                leftAndRightLookAngle = transform.eulerAngles.y;
                upAndDownLookAngle = transform.eulerAngles.x;
            }
            // Normal camera rotation
            else
            {
                // If locked on, force rotation towards the target
                // else rotate regularly
                Quaternion targetRotation;

                // Normal rotations
                // Rotate left and right based on horizontal movement on the right joystick
                leftAndRightLookAngle += PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed * Time.deltaTime;
                // Rotate up and down based on vertical movement on the right joystick
                upAndDownLookAngle -= PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed * Time.deltaTime;

                // Clamp the up and down look angle between a min and max value
                upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

                Vector3 cameraRotation;
                #region Rotate this GameObject left and right
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
            }
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
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;

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
                            avaliableTargets.Add(lockOnTarget);
                        }
                    }
                }
            }

            foreach (var target in avaliableTargets)
            {
                if (target != null)
                {
                    float distanceFromTarget = Vector3.Distance(player.transform.position, target.transform.position);

                    if (distanceFromTarget < shortestDistance)
                    {
                        shortestDistance = distanceFromTarget;
                        nearestLockOnTarget = target;
                    }

                    // If we are already locked on
                    if (player.isLockedOn)
                    {
                        Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(target.transform.position);

                        var distanceFromLeftTarget = relativeEnemyPosition.x;
                        var distanceFromRightTarget = relativeEnemyPosition.x;

                        if (target == player.playerCombatManager.currentTarget) continue;

                        if (relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceOfLeftTarget)
                        {
                            shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                            leftLockOnTarget = target;
                        }
                        else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceOfRightTarget)
                        {
                            shortestDistanceOfRightTarget = distanceFromRightTarget;
                            rightLockOnTarget = target;
                        }
                    }
                }
                else
                {
                    ClearLockOnTargets();
                    player.isLockedOn = false;
                }
            }
        }

        public void SetLockCameraHeight()
        {
            if (cameraLockOnHeightCoroutine != null)
            {
                StopCoroutine(cameraLockOnHeightCoroutine);
            }

            cameraLockOnHeightCoroutine = StartCoroutine(SetCameraHeight());
        }

        public void ClearLockOnTargets()
        {
            nearestLockOnTarget = null;
            leftLockOnTarget = null;
            rightLockOnTarget = null;
            avaliableTargets.Clear();
        }

        public IEnumerator WaitThenFindNewTarget()
        {
            while (player.isPerformingAction)
            {
                yield return null;
            }

            ClearLockOnTargets();
            HandleLocatingLockOnTargets();

            if (nearestLockOnTarget != null)
            {
                player.playerCombatManager.SetTarget(nearestLockOnTarget);
                player.isLockedOn = true;
            }

            yield return null;
        }

        private IEnumerator SetCameraHeight()
        {
            float duration = 1f;
            float timer = 0;

            Vector3 velocity = Vector3.zero;
            Vector3 newLockedCameraHeight = new(cameraPivotTransform.localPosition.x, lockedCameraHeight);
            Vector3 newUnlockedCameraHeight = new(cameraPivotTransform.localPosition.x, unlockedCameraHeight);

            while (timer < duration)
            {
                timer += Time.deltaTime;

                if (player != null)
                {
                    if (player.playerCombatManager.currentTarget != null)
                    {
                        cameraPivotTransform.SetLocalPositionAndRotation(Vector3.SmoothDamp(cameraPivotTransform.localPosition, newLockedCameraHeight, ref velocity, setCameraHeightSpeed),
                        Quaternion.Slerp(cameraPivotTransform.localRotation, Quaternion.Euler(0, 0, 0), lockOnTargetFollowSpeed));
                    }
                    else
                    {
                        cameraPivotTransform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newUnlockedCameraHeight, ref velocity, setCameraHeightSpeed);
                    }
                }

                yield return null;
            }

            if (player != null)
            {
                if (player.playerCombatManager.currentTarget != null)
                {
                    cameraPivotTransform.SetLocalPositionAndRotation(newLockedCameraHeight, Quaternion.Euler(0, 0, 0));
                }
                else
                {
                    cameraPivotTransform.localPosition = newUnlockedCameraHeight;
                }
            }

            yield return null;
        }
    }
}