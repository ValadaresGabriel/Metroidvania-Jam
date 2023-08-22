using UnityEngine;
using System.Collections;

namespace IM
{
    public class PlayerCamera : MonoBehaviour
    {
        public static PlayerCamera Instance { get; private set; }

        [HideInInspector]
        public PlayerManager player;

        [SerializeField]
        public Camera cameraObject;

        [SerializeField]
        private Transform cameraPivotTransform;

        [Header("Camera Settings")]

        private float cameraSmoothSpeed = 1; // The bigger this number is, the longer for the camera to reach its position during movement

        [SerializeField]
        private float leftAndRightRotationSpeed = 220f;

        [SerializeField]
        private float upAndDownRotationSpeed = 220f;

        [SerializeField]
        private float minimumPivot = -30f; // The lowest point you are able to look down

        [SerializeField]
        private float maximumPivot = 60f; // The highest point you are able to look down

        [SerializeField]
        private float cameraCollisionRadius = 0.2f; // The highest point you are able to look down

        [SerializeField]
        private LayerMask collideWithLayers;

        [Header("Camera Values")]

        private Vector3 cameraVelocity;

        private Vector3 cameraObjectPosition; // Used for camera collisions (moves the camera object to this position)

        [SerializeField]
        private float leftAndRightLookAngle;

        [SerializeField]
        private float upAndDownLookAngle;

        private float cameraZPosition; // Value user for camera collision

        private float targetCameraZPosition; // Value user for camera collision

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
            // If locked on, force rotation towards the target
            // else rotate regularly

            // Normal rotations
            // Rotate left and right based on horizontal movement on the right joystick
            leftAndRightLookAngle += PlayerInputManager.Instance.cameraHorizontalInput * leftAndRightRotationSpeed * Time.deltaTime;
            // Rotate up and down based on vertical movement on the right joystick
            upAndDownLookAngle -= PlayerInputManager.Instance.cameraVerticalInput * upAndDownRotationSpeed * Time.deltaTime;

            // Clamp the up and down look angle between a min and max value
            upAndDownLookAngle = Mathf.Clamp(upAndDownLookAngle, minimumPivot, maximumPivot);

            Vector3 cameraRotation;
            Quaternion targetRotation;
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