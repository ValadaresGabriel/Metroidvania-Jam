using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

namespace TS
{
    public class AmbushState : State
    {
        [SerializeField]
        private bool isSleeping;

        [SerializeField]
        private string sleepAnimation;

        [SerializeField]
        private string wakeAnimation;

        [SerializeField]
        private LayerMask detectionLayer;

        [SerializeField]
        private PursueTargetState pursueTargetState;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (isSleeping && enemyManager.isPerformingAction == false)
            {
                enemyAnimatorManager.PlayTargetActionAnimation(sleepAnimation, true);
            }

            #region Handle Target Detection

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
                        isSleeping = false;
                        enemyAnimatorManager.PlayTargetActionAnimation(wakeAnimation, true);
                    }
                }
            }

            #endregion

            #region Handle State Update

            if (enemyManager.currentTarget != null)
            {
                return pursueTargetState;
            }

            return this;

            #endregion
        }
    }
}
