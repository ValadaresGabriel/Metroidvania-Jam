using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class PursueTargetState : State
    {
        [SerializeField]
        private CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.isPerformingAction) return this;

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            enemyManager.SetDistanceFromTarget(Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position));
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (enemyManager.GetDistanceFromTarget() > enemyManager.GetMaximumAttackRange())
            {
                enemyManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotationToTarget(enemyManager);

            enemyManager.navMeshAgent.transform.position = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (enemyManager.GetDistanceFromTarget() <= enemyManager.GetMaximumAttackRange())
            {
                return combatStanceState;
            }

            return this;
        }

        private void HandleRotationToTarget(EnemyManager enemyManager)
        {
            Vector3 directionToTarget = enemyManager.currentTarget.transform.position - transform.position;
            directionToTarget.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, Time.deltaTime * enemyManager.GetRotationSpeed());

            if (!enemyManager.isPerformingAction)
            {
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
            }
        }
    }
}
