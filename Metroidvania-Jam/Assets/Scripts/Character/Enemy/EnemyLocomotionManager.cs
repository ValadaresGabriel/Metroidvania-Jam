using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class EnemyLocomotionManager : CharacterLocomotionManager
    {
        private EnemyManager enemyManager;

        // [HideInInspector]
        public CharacterStatsManager currentTarget;

        [SerializeField]
        private LayerMask detectionLayer;

        protected override void Awake()
        {
            enemyManager = GetComponent<EnemyManager>();
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
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.detectionRadius, detectionLayer);

            foreach (Collider collider in colliders)
            {
                if (collider.transform.TryGetComponent(out CharacterStatsManager characterStatsManager))
                {
                    Vector3 targetDirection = characterStatsManager.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.minimumDetectionAngle && viewableAngle < enemyManager.maximumDetectionAngle)
                    {
                        currentTarget = characterStatsManager;
                    }
                }
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, enemyManager.detectionRadius);
        }
    }
}
