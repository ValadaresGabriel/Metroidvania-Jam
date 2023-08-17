using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class IdleState : State
    {
        public override State Tick(EnemyManager enemyManager, EnemyStatsManager enemyStatsManager, EnemyAnimatorManager enemyAnimatorManager)
        {
            return this;
        }
    }
}
