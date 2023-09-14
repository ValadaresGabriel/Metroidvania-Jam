using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    [CreateAssetMenu(menuName = "Character Actions/Weapon Actions/Light Attack Action")]
    public class LightAttackWeaponItemAction : WeaponItemAction
    {
        [SerializeField] private string light_attack_01 = "Main_Light_Attack_01";
        // Temporary Combo
        [SerializeField] private string light_attack_02 = "Main_Light_Attack_02";
        [SerializeField] private string light_attack_03 = "Main_Light_Attack_03";

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            // Check for stops. Example: stamina <= 0 (will not be in this demo)

            if (!playerPerformingAction.isGrounded) return;

            if (playerPerformingAction.canDoCombo)
            {
                PerformLightAttackCombo(playerPerformingAction, weaponPerformingAction);
            }
            else
            {
                PerformLightAttack(playerPerformingAction, weaponPerformingAction);
            }
        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.isUsingRightHand)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack01, light_attack_01, true);
            }
        }

        private void PerformLightAttackCombo(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.isUsingRightHand)
            {
                switch (playerPerformingAction.playerCombatManager.currentAttackType)
                {
                    case AttackType.LightAttack01:
                        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack02, light_attack_02, true);
                        break;
                    case AttackType.LightAttack02:
                        playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(AttackType.LightAttack03, light_attack_03, true);
                        break;
                }

                playerPerformingAction.playerCombatManager.CloseCanDoCombo();
            }
        }
    }
}
