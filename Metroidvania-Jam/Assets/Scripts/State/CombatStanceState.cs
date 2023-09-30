using UnityEngine;

namespace TS
{
    public class CombatStanceState : State
    {
        [SerializeField]
        private PursueTargetState pursueTargetState;

        [SerializeField]
        private AttackState attackState;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.IsInteracting) return this;

            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);

            if (enemyManager.isPerformingAction)
            {
                enemyAnimatorManager.UpdateAnimatorMovementParameters(0, 0);
            }

            if (enemyManager.currentRecoveryTime <= 0 && distanceFromTarget <= enemyManager.GetMaximumAttackRange())
            {
                return attackState;
            }
            else if (distanceFromTarget > enemyManager.GetMaximumAttackRange())
            {
                return pursueTargetState;
            }

            return this;
        }
    }
}
