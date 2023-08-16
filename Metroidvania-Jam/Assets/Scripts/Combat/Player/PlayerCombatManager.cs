using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class PlayerCombatManager : CharacterCombatManager
    {
        private void HandleLockOn()
        {
            Collider[] enemyColliders = Physics.OverlapSphere(transform.position, detectionRadius, targetLayer);

            if (enemyColliders.Length <= 0) return;

            foreach (var enemy in enemyColliders)
            {
                CharacterManager character = enemy.GetComponent<CharacterManager>();

                if (character != null)
                {

                }
            }
        }
    }
}
