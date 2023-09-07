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

        public override void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            base.AttemptToPerformAction(playerPerformingAction, weaponPerformingAction);

            // Check for stops. Example: stamina <= 0 (will not be in this demo)

            if (!playerPerformingAction.isGrounded) return;

            PerformLightAttack(playerPerformingAction, weaponPerformingAction);
        }

        private void PerformLightAttack(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            if (playerPerformingAction.isUsingRightHand)
            {
                playerPerformingAction.playerAnimatorManager.PlayTargetAttackActionAnimation(light_attack_01, true);
            }
        }
    }
}
