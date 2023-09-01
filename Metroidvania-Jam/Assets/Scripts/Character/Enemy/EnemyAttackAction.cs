using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

namespace TS
{
    [CreateAssetMenu(menuName = "A.I./Enemy Actions/Attack Action")]
    public class EnemyAttackAction : EnemyAction
    {
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
    }
}
