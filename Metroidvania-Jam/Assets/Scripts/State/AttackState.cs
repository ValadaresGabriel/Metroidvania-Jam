using UnityEngine;

namespace TS
{
    public class AttackState : State
    {
        [SerializeField] private CombatStanceState combatStanceState;
        [SerializeField] private EnemyAttackAction[] enemyAttacks;
        [SerializeField] private EnemyAttackAction currentAttack;

        private bool willDoComboOnNextAttack = false;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            if (enemyManager.IsInteracting && enemyManager.canDoCombo == false)
            {
                return this;
            }
            else if (enemyManager.IsInteracting && enemyManager.canDoCombo)
            {
                if (willDoComboOnNextAttack)
                {
                    willDoComboOnNextAttack = false;
                    enemyAnimatorManager.PlayTargetAttackActionAnimation(currentAttack.attackType, currentAttack.ActionAnimation, true);
                }
            }

            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            HandleRotationToTarget(enemyManager);

            if (enemyManager.isPerformingAction && willDoComboOnNextAttack == false)
            {
                return combatStanceState;
            }

            if (currentAttack != null)
            {
                if (distanceFromTarget < currentAttack.MinimumDistanceNeededToAttack)
                {
                    return this;
                }
                else if (distanceFromTarget < currentAttack.MaximumDistanceNeededToAttack)
                {
                    if (viewableAngle <= currentAttack.MaximumAttackAngle && viewableAngle >= currentAttack.MinimumAttackAngle)
                    {
                        if (enemyManager.currentRecoveryTime <= 0 && enemyManager.isPerformingAction == false)
                        {
                            enemyAnimatorManager.UpdateAnimatorMovementParameters(0, 0);
                            enemyAnimatorManager.PlayTargetActionAnimation(currentAttack.ActionAnimation, true);
                            enemyManager.isPerformingAction = true;
                            RollForComboChance(enemyManager);

                            if (currentAttack.canCombo && willDoComboOnNextAttack)
                            {
                                currentAttack = currentAttack.attackCombo;
                                return this;
                            }
                            else
                            {
                                enemyManager.currentRecoveryTime = currentAttack.RecoveryTime;
                                currentAttack = null;

                                return combatStanceState;
                            }
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

        private void RollForComboChance(EnemyManager enemyManager)
        {
            float comboChance = Random.Range(0f, 1f);

            if (enemyManager.allowAIToPerformCombo && comboChance <= enemyManager.comboLikelyHood)
            {
                willDoComboOnNextAttack = true;
            }
        }

        private void GetNewAttack(EnemyManager enemyManager)
        {
            Vector3 targetsDirection = enemyManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, transform.position);

            int maxScore = 0;

            foreach (EnemyAttackAction attack in enemyAttacks)
            {
                if (distanceFromTarget <= attack.MaximumDistanceNeededToAttack && distanceFromTarget >= attack.MinimumDistanceNeededToAttack)
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
                if (distanceFromTarget <= attack.MaximumDistanceNeededToAttack && distanceFromTarget >= attack.MinimumDistanceNeededToAttack)
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
