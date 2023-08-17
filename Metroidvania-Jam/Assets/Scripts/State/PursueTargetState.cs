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
            if (enemyManager.isPerformingAction)
            {
                Vector3 direction = enemyManager.currentTarget.transform.position - transform.position;
                direction.y = 0;
                direction.Normalize();

                if (direction == Vector3.zero)
                {
                    direction = transform.forward;
                }

                Quaternion targetRotation = Quaternion.LookRotation(direction);
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.GetRotationSpeed() / Time.deltaTime);
            }
            else
            {
                Vector3 relativeDirection = transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
                Vector3 targetVelocity = enemyManager.RB.velocity;

                enemyManager.navMeshAgent.enabled = true;
                enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
                enemyManager.RB.velocity = targetVelocity;
                enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.GetRotationSpeed() / Time.deltaTime);
            }
        }
    }
}
