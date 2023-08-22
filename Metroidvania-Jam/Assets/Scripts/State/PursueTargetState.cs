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
            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);
                return this;
            }

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, enemyManager.transform.forward);

            if (distanceFromTarget > enemyManager.GetMaximumAttackRange())
            {
                enemyManager.animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
            }

            HandleRotationToTarget(enemyManager);

            enemyManager.navMeshAgent.transform.position = Vector3.zero;
            enemyManager.navMeshAgent.transform.localRotation = Quaternion.identity;

            if (distanceFromTarget <= enemyManager.GetMaximumAttackRange())
            {
                return combatStanceState;
            }

            return this;
        }

        // private void HandleRotationToTarget(EnemyManager enemyManager)
        // {
        //     if (enemyManager.isPerformingAction)
        //     {
        //         Vector3 direction = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
        //         direction.y = 0;
        //         direction.Normalize();

        //         if (direction == Vector3.zero)
        //         {
        //             direction = enemyManager.transform.forward;
        //         }

        //         Quaternion targetRotation = Quaternion.LookRotation(direction);
        //         enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, targetRotation, enemyManager.GetRotationSpeed() / Time.deltaTime);
        //     }
        //     else
        //     {
        //         Vector3 relativeDirection = enemyManager.transform.InverseTransformDirection(enemyManager.navMeshAgent.desiredVelocity);
        //         Vector3 targetVelocity = enemyManager.RB.velocity;

        //         enemyManager.navMeshAgent.enabled = true;
        //         enemyManager.navMeshAgent.SetDestination(enemyManager.currentTarget.transform.position);
        //         enemyManager.RB.velocity = targetVelocity;
        //         enemyManager.transform.rotation = Quaternion.Slerp(enemyManager.transform.rotation, enemyManager.navMeshAgent.transform.rotation, enemyManager.GetRotationSpeed() / Time.deltaTime);
        //     }
        // }

        private void HandleRotationToTarget(EnemyManager enemyManager)
        {
            Vector3 directionToTarget = enemyManager.currentTarget.transform.position - enemyManager.transform.position;
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
