using System.Collections;
using System.Collections.Generic;
using TS;
using UnityEngine;

public class EnemyCombatManager : CharacterCombatManager
{
    protected override void Awake()
    {
        base.Awake();

        currentAttackType = AttackType.NONE;
    }
}
