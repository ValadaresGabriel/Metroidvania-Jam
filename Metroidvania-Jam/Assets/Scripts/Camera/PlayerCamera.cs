using UnityEngine;
using System.Collections;
using Unity.Mathematics;
using System.Collections.Generic;

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

        [Header("Lock On Settings")]
        [SerializeField] private LayerMask lockOnTargetLayerMask;
        [SerializeField] private float lockOnMaximumDistance = 30;
        [SerializeField] private CharacterManager lockOnCurrentTarget;
        [SerializeField] private CharacterManager lockOnNearestTarget;
        [SerializeField] private CharacterManager lockOnLeftTarget;
        [SerializeField] private CharacterManager lockOnRightTarget;
        // [SerializeField] private Transform shortestDistanceOfLeftTarget;
        [SerializeField] private float lockedPivotPosition = 2.25f;
        [SerializeField] private float unlockedPivotPosition = 1.65f;
        private List<CharacterManager> lockOnAvailableTargets = new();

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

            #region Locked On Camera Rotation
            if (player.isLockedOnEnemy && lockOnCurrentTarget != null)
            {
                float velocity = 0;

                Vector3 direction = lockOnCurrentTarget.transform.position - transform.position;
                direction.Normalize();
                direction.y = 0;

                targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = targetRotation;

                direction = lockOnCurrentTarget.transform.position - cameraPivotTransform.position;
                direction.Normalize();

                targetRotation = Quaternion.LookRotation(direction);
                Vector3 eulerAngle = targetRotation.eulerAngles;
                eulerAngle.y = 0;

                cameraPivotTransform.localEulerAngles = eulerAngle;

                return;
            }
            #endregion

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

        public void AttemptToLockOn()
        {
            HandleLockOn();

            if (lockOnNearestTarget != null)
            {
                player.isLockedOnEnemy = true;
                lockOnCurrentTarget = lockOnNearestTarget;
            }
        }

        public void AttemptToSetLockOnBasedOnLeftOrRightTarget(LockOnRightLeftTarget lockOnTarget)
        {
            HandleLockOn();

            if (lockOnTarget == LockOnRightLeftTarget.Left)
            {
                if (lockOnLeftTarget != null)
                {
                    lockOnCurrentTarget = lockOnLeftTarget;
                }
            }
            else
            {
                if (lockOnRightTarget != null)
                {
                    lockOnCurrentTarget = lockOnRightTarget;
                }
            }
        }

        private void HandleLockOn()
        {
            float shortestDistance = Mathf.Infinity;
            float shortestDistanceOfLeftTarget = -Mathf.Infinity;
            float shortestDistanceOfRightTarget = Mathf.Infinity;

            foreach (Collider targetCollider in Physics.OverlapSphere(player.transform.position, 26, lockOnTargetLayerMask))
            {
                if (targetCollider.TryGetComponent(out CharacterManager character))
                {
                    Vector3 lockTargetDirection = character.transform.position - player.transform.position;
                    float distanceFormTarget = Vector3.Distance(player.transform.position, character.transform.position);
                    float viewableAngle = Vector3.Angle(lockTargetDirection, cameraObject.transform.forward);
                    RaycastHit hit;

                    if (character.transform.root != player.transform.root && viewableAngle > -50 && viewableAngle < 50 && distanceFormTarget <= lockOnMaximumDistance)
                    {
                        if (Physics.Linecast(player.lockOnTransform.position, character.lockOnTransform.position, out hit))
                        {
                            Debug.DrawLine(player.lockOnTransform.position, character.lockOnTransform.position);

                            if (hit.transform.gameObject.layer == environmentLayer)
                            {
                                // Cannot lock onto target, object in the way
                            }
                            else
                            {
                                lockOnAvailableTargets.Add(character);
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogWarning($"CHARACTER MANAGER CANNOT BE FOUND ON: {targetCollider}");
                }
            }

            foreach (CharacterManager target in lockOnAvailableTargets)
            {
                float distanceFormTarget = Vector3.Distance(player.transform.position, target.transform.position);

                if (distanceFormTarget < shortestDistance)
                {
                    shortestDistance = distanceFormTarget;
                    lockOnNearestTarget = target;
                }

                if (player.isLockedOnEnemy)
                {
                    // Vector3 relativeEnemyPosition = lockOnCurrentTarget.transform.InverseTransformPoint(target.transform.position);
                    // var distanceFromLeftTarget = lockOnCurrentTarget.transform.position.x - target.transform.position.x;
                    // var distanceFromRightTarget = lockOnCurrentTarget.transform.position.x + target.transform.position.x;

                    Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(target.transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    if (relativeEnemyPosition.x <= 0 && distanceFromLeftTarget > shortestDistanceOfLeftTarget && target != lockOnCurrentTarget)
                    {
                        shortestDistanceOfLeftTarget = distanceFromLeftTarget;
                        lockOnLeftTarget = target;
                    }
                    else if (relativeEnemyPosition.x >= 0 && distanceFromLeftTarget < shortestDistanceOfRightTarget && target != lockOnCurrentTarget)
                    {
                        shortestDistanceOfRightTarget = distanceFromRightTarget;
                        lockOnRightTarget = target;
                    }
                }
            }
        }

        public void ClearLockOnTargets()
        {
            lockOnAvailableTargets.Clear();
            lockOnNearestTarget = null;
            lockOnCurrentTarget = null;
        }

        public void SetCameraHeight()
        {
            Vector3 velocity = Vector3.zero;
            Vector3 newLockedPosition = new Vector3(0, lockedPivotPosition);
            Vector3 newUnlockedPosition = new Vector3(0, unlockedPivotPosition);

            if (lockOnCurrentTarget != null)
            {
                cameraPivotTransform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newLockedPosition, ref velocity, Time.deltaTime);
            }
            else
            {
                cameraPivotTransform.localPosition = Vector3.SmoothDamp(cameraPivotTransform.localPosition, newUnlockedPosition, ref velocity, Time.deltaTime);
            }
        }

        public Transform GetLockOnCurrentTarget() => lockOnCurrentTarget.transform;
    }
    // public class PlayerCamera : MonoBehaviour
    // {
    //     [SerializeField]
    //     private Transform target;

    //     [SerializeField]
    //     private float smoothing = 5f;

    //     private Vector3 offset;

    //     void Start()
    //     {
    //         offset = transform.position - target.position;
    //     }

    //     void LateUpdate()
    //     {
    //         Vector3 targetCamPos = target.position + offset;
    //         transform.position = Vector3.Lerp(transform.position, targetCamPos, smoothing * Time.deltaTime);
    //     }
    // }
}