using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Timeline;

namespace IM
{
    public class EnemyLocomotionManager : CharacterLocomotionManager
    {
        private EnemyManager enemy;

        // [HideInInspector]
        public CharacterStatsManager currentTarget;

        [SerializeField]
        private LayerMask detectionLayer;

        [SerializeField]
        private float distanceFromTarget;

        [SerializeField]
        private float stoppingDistance = 1f;

        [SerializeField]
        private float rotationSpeed = 15f;

        public float SetDistanceFromTarget(float newDistance) => distanceFromTarget = newDistance;

        public float GetDistanceFromTarget() => distanceFromTarget;

        public float GetStoppingDistance() => stoppingDistance;

        protected override void Awake()
        {
            enemy = GetComponent<EnemyManager>();
        }

        protected override void Update()
        {
            // if (currentTarget == null)
            // {
            //     HandleDetection();
            // }
        }

        public void HandleDetection()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemy.detectionRadius, detectionLayer);

            foreach (Collider collider in colliders)
            {
                if (collider.transform.TryGetComponent(out CharacterStatsManager characterStatsManager))
                {
                    Vector3 targetDirection = characterStatsManager.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemy.minimumDetectionAngle && viewableAngle < enemy.maximumDetectionAngle)
                    {
                        currentTarget = characterStatsManager;
                    }
                }
            }
        }

        public void HandleMoveToTarget()
        {
            if (enemy.isPerformingAction) return;

            Vector3 targetDirection = currentTarget.transform.position - transform.position;
            distanceFromTarget = Vector3.Distance(currentTarget.transform.position, transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (enemy.isPerformingAction)
            {
                // Action logic like attack
                enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                enemy.navMeshAgent.enabled = false;
            }
            else
            {
                if (distanceFromTarget > stoppingDistance)
                {
                    enemy.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                }
                else if (distanceFromTarget <= stoppingDistance)
                {
                    enemy.animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                }
            }

            HandleRotationToTarget();

            enemy.navMeshAgent.transform.position = Vector3.zero;
            enemy.navMeshAgent.transform.localRotation = Quaternion.identity;
        }

        private void HandleRotationToTarget()
        {
            if (enemy.isPerformingAction)
            {
                Vector3 direction = currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed / Time.deltaTime);
            }
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(enemy.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemy.RB.velocity;

                enemy.navMeshAgent.enabled = true;
                enemy.navMeshAgent.SetDestination(currentTarget.transform.position);
                enemy.RB.velocity = targetVelocity;
                transform.rotation = Quaternion.Slerp(transform.rotation, enemy.navMeshAgent.transform.rotation, rotationSpeed / Time.deltaTime);
            }
        }

        // private void OnDrawGizmosSelected()
        // {
        //     Gizmos.DrawWireSphere(transform.position, enemy.detectionRadius);
        // }
    }
}
