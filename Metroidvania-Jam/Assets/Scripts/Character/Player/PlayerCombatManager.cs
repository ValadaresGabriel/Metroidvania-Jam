using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private PlayerManager player;
        public WeaponItem currentWeaponBeingUsed;

        protected override void Awake()
        {
            base.Awake();

            player = GetComponent<PlayerManager>();
        }

        public void PerformWeaponBasedAction(WeaponItemAction weaponAction, WeaponItem weaponPerformingAction)
        {
            // Perform the Action
            weaponAction.AttemptToPerformAction(player, weaponPerformingAction);

            // player.PerformWeaponBasedAction(weaponAction.actionID, weaponPerformingAction.itemID);
        }
    }
}
