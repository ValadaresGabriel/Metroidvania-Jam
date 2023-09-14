using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class PursueTargetState : State
    {
        [SerializeField]
        private CombatStanceState combatStanceState;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.IsInteracting) return this;

            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.UpdateAnimatorMovementParameters(0, 0);
                return this;
            }

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            enemyManager.distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            if (enemyManager.distanceFromTarget > enemyManager.GetMaximumAttackRange())
            {
                enemyManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotationToTarget(enemyManager);

            if (enemyManager.distanceFromTarget <= enemyManager.GetMaximumAttackRange())
            {
                return combatStanceState;
            }
            else
            {
                return this;
            }
        }

        private void HandleRotationToTarget(EnemyManager enemyManager)
        {
            Vector3 directionToTarget = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            directionToTarget.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.GetRotationSpeed() / Time.deltaTime);

            if (!enemyManager.isPerformingAction)
            {
                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.GetRotationSpeed() / Time.deltaTime);
            }
        }
    }
}
