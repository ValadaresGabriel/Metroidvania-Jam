using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class EnemyCombatManager : CharacterCombatManager
    {
        private PlayerManager player;
        public WeaponItem currentWeaponBeingUsed;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();

            currentAttackType = AttackType.NONE;
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            if (player.isPerformingAction && player.canDoCombo == false) return;

            // Perform the Action
            weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

            // player.PerformWeaponBasedAction(weaponAction.actionID, weaponPerformingAction.itemID);
        }

        public virtual void DrainStaminaBasedOnAttack()
        {
            float staminaDeducted = 0;

            if (currentWeaponBeingUsed == null) return;

            switch (currentAttackType)
            {
                case AttackType.LightAttack01:
                    staminaDeducted = currentWeaponBeingUsed.baseStaminaCost * currentWeaponBeingUsed.lightAttackStaminaCostMultiplier;
                    break;
                default:
                    break;
            }

            player.UpdateCharacterStamina(staminaDeducted);
        }
    }
}
