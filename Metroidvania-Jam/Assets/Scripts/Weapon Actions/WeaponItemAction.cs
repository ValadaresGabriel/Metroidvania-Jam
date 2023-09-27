using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class WeaponItemAction : ScriptableObject
    {
        public int actionID;

        public virtual void AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction, bool isHeavyAttack = false, bool isHeavyAttackFull = false)
        {
            // What does every weapon action have in common?
            playerPerformingAction.OnCurrentWeaponBeingUsedIDChange(weaponPerformingAction.itemID);
        }

        public virtual IEnumerator AttemptToPerformAction(PlayerManager playerPerformingAction, WeaponItem weaponPerformingAction)
        {
            // What does every weapon action have in common?
            playerPerformingAction.OnCurrentWeaponBeingUsedIDChange(weaponPerformingAction.itemID);

            yield return null;
        }
    }
}
