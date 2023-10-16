using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private PlayerManager player;
        public WeaponItem currentWeaponBeingUsed;
        public float heavyAttackMultiplier;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction, bool isHeavyAttack = false, bool isHeavyAttackFull = false)
        {
            if (player.isPerformingAction && player.canDoCombo == false && isHeavyAttack == false) return;

            player.playerLocomotionManager.HandleRotation();

            // Perform the Action
            weaponAction.AttemptToPerformAction(player, weaponPerformingAction, isHeavyAttack, isHeavyAttackFull);

            // player.PerformWeaponBasedAction(weaponAction.actionID, weaponPerformingAction.itemID);
        }

        public override void SetTarget(CharacterManager newTarget)
        {
            base.SetTarget(newTarget);

            PlayerCamera.Instance.SetLockCameraHeight();
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
