using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class IdleState : State
    {
        [SerializeField]
        private PursueTargetState pursueTargetState;

        [SerializeField]
        private LayerMask detectionLayer;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            #region Handle Enemy Target Detection
            Collider[] colliders = Physics.OverlapSphere(transform.position, enemyManager.GetDetectionRadius(), detectionLayer);

            foreach (Collider collider in colliders)
            {
                if (collider.transform.TryGetComponent(out CharacterStatsManager characterStatsManager))
                {
                    Vector3 targetDirection = characterStatsManager.transform.position - transform.position;
                    float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

                    if (viewableAngle > enemyManager.GetMinimumDetectionAngle() && viewableAngle < enemyManager.GetMaximumDetectionAngle())
                    {
                        enemyManager.currentTarget = characterStatsManager;
                        return pursueTargetState;
                    }
                }
            }
            #endregion

            #region Handle Switching to Next State
            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }

            return this;
            #endregion
        }
    }
}
