using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace IM
{
    public class CombatStanceState : State
    {
        [SerializeField]
        private PursueTargetState pursueTargetState;

        [SerializeField]
        private AttackState attackState;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            enemyManager.SetDistanceFromTarget(Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position));

            if (enemyManager.currentRecoveryTime <= 0 && enemyManager.GetDistanceFromTarget() <= enemyManager.GetMaximumAttackRange())
            {
                return attackState;
            }
            else if (enemyManager.GetDistanceFromTarget() > enemyManager.GetMaximumAttackRange())
            {
                return pursueTargetState;
            }
            else
            {
                return this;
            }
        }
    }
}
