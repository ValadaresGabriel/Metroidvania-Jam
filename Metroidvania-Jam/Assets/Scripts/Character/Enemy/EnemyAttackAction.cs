using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    [CreateAssetMenu(menuName = "A.I./Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
        [Header("Attack Type")]
        public AttackType attackType = AttackType.LightAttack01;

        [Header("Attack Score")]
        [Tooltip("The higher, the more the probability of this attack happen")]
        public int AttackScore = 3;

        [Header("Attack Cooldown (sort of)")]
        public float RecoveryTime = 2;

        [Header("Angle To Attack")]
        public float MaximumAttackAngle = 35;
        public float MinimumAttackAngle = -35;

        [Header("Range To Attack")]
        public float MinimumDistanceNeededToAttack = 0;
        public float MaximumDistanceNeededToAttack = 3;

        [Header("Combo")]
        public bool canCombo = true;
        public EnemyAttackAction attackCombo;
    }
}
