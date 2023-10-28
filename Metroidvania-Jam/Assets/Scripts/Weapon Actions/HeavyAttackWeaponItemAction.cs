using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TS
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Heavy Attack Action")]
    public class HeavyAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] private string heavy_attack_01 = "Main_Heavy_Attack_01";
        [SerializeField] private string heavy_attack_02 = "Main_Heavy_Attack_02";
        [SerializeField] private string heavy_attack_03 = "Main_Heavy_Attack_03";

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            // Check for stops. Example: stamina <= 0 (will not be in this demo)

            if (!playerPerformingAction.isGrounded) return;

            // Temporary Heavy Attack
            if (playerPerformingAction.canDoCombo)
            {
                PerformHeavyAttackCombo(playerPerformingAction, weaponPerformingAction);
            }
            else
            {
                PerformHeavyAttack(playerPerformingAction, weaponPerformingAction);
            }
        }

        private void PerformHeavyAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.isUsingRightHand)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack01, heavy_attack_01, true);
            }
        }

        private void PerformHeavyAttackCombo(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.isUsingRightHand)
            {
                switch (playerPerformingAction.playerCombatManager.currentAttackType)
                {
                    case AttackType.HeavyAttack01:
                        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack02, heavy_attack_02, true);
                        break;
                    case AttackType.HeavyAttack02:
                        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.HeavyAttack03, heavy_attack_03, true);
                        break;
                }

                playerPerformingAction.playerCombatManager.CloseCanDoCombo();
            }
        }
    }
}
