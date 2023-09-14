using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList.Element_Adder_Menu;
using UnityEngine;

namespace TS
{
    public class AttackState : State
    {
        [SerializeField] private CombatStanceState combatStanceState;
        [SerializeField] private EnemyAttackAction[] enemyAttacks;
        [SerializeField] private EnemyAttackAction currentAttack;

        [SerializeField] private string light_attack_01 = "Main_Light_Attack_01";
        // Temporary Combo
        [SerializeField] private string light_attack_02 = "Main_Light_Attack_02";
        [SerializeField] private string light_attack_03 = "Main_Light_Attack_03";

        private bool willDoComboOnNextAttack = false;

        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            Vector3 targetDirection = enemyManager.currentTarget.transform.position - transform.position;
            float distanceFromTarget = Vector3.Distance(enemyManager.currentTarget.transform.position, enemyManager.transform.position);
            float viewableAngle = Vector3.Angle(targetDirection, transform.forward);

            if (enemyManager.isPerformingAction && willDoComboOnNextAttack == false)
            {
                return combatStanceState;
            }
            else if (willDoComboOnNextAttack && enemyManager.canDoCombo)
            {
                willDoComboOnNextAttack = false;
                enemyAnimatorManager.PlayTargetAttackActionAnimation(currentAttack.attackType, currentAttack.ActionAnimation, true);
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
                            // enemyAnimatorManager.PlayTargetAttackActionAnimation(currentAttack.attackType, currentAttack.ActionAnimation, true);
                            PerformAttack(enemyManager);
                            RollForComboChance(enemyManager);

                            if (currentAttack.canCombo && willDoComboOnNextAttack && enemyManager.canDoCombo)
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

        private void PerformAttack(EnemyManager enemyPerformingAction)
        {
            enemyPerformingAction.enemyAnimatorManager.UpdateAnimatorMovementParameters(0, 0, false);

            switch (enemyPerformingAction.enemyCombatManager.currentAttackType)
            {
                case AttackType.NONE:
                    enemyPerformingAction.enemyAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_attack_01, true);
                    break;
                case AttackType.LightAttack01:
                    enemyPerformingAction.enemyAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack02, light_attack_02, true);
                    break;
                case AttackType.LightAttack02:
                    enemyPerformingAction.enemyAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack03, light_attack_03, true);
                    break;
            }

            enemyPerformingAction.enemyCombatManager.CloseCanDoCombo();
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
