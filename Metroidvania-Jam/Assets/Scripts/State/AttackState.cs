using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace IM
{
    public class AttackState : State
    {
        [SerializeField]
        private CombatStanceState combatStanceState;

        [SerializeField]
        private EnemyAttackAction[] enemyAttacks;

        [SerializeField]
        private EnemyAttackAction currentAttack;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (enemyManager.isPerformingAction) return combatStanceState;

            if (currentAttack != null)
            {
                if (enemyManager.GetDistanceFromTarget() < currentAttack.MinimumDistanceNeededToAttack)
                {
                    return this;
                }
                else if (enemyManager.GetDistanceFromTarget() < currentAttack.MaximumDistanceNeededToAttack)
                {
                    if (enemyManager.GetViewableAngle() <= currentAttack.MaximumAttackAngle && enemyManager.GetViewableAngle() >= currentAttack.MinimumAttackAngle)
                    {
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                        {
                            enemyAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);
                            enemyAnimatorManager.PlayTargetActionAnimation(currentAttack.ActionAnimation, true);

                            enemyManager.currentRecoveryTime = currentAttack.RecoveryTime;
                            currentAttack = null;

                            return combatStanceState;
                        }
                    }
                }
            }
            else
            {
                GetNewAttack(enemyManager);
            }

            return combatStanceState;
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);

            enemyManager.SetDistanceFromTarget(Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position));

            int maxScore = 0;

            foreach (EnemyAttackAction attack in enemyAttacks)
            {
                if (enemyManager.GetDistanceFromTarget() <= attack.MaximumDistanceNeededToAttack && enemyManager.GetDistanceFromTarget() >= attack.MinimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= attack.MaximumAttackAngle && viewableAngle >= attack.MinimumDistanceNeededToAttack)
                    {
                        maxScore += attack.AttackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            foreach (EnemyAttackAction attack in enemyAttacks)
            {
                if (enemyManager.GetDistanceFromTarget() <= attack.MaximumDistanceNeededToAttack && enemyManager.GetDistanceFromTarget() >= attack.MinimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= attack.MaximumAttackAngle && viewableAngle >= attack.MinimumDistanceNeededToAttack)
                    {
                        if (currentAttack != null) return;

                        temporaryScore += attack.AttackScore;

                        if (temporaryScore > randomValue)
                        {
                            currentAttack = attack;
                        }
                    }
                }
            }
        }
    }
}
