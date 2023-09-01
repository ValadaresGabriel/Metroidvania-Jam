using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class EnemyAnimatorManager : CharacterAnimatorManager
    {
        private EnemyManager enemy;

        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            enemy = GetComponent<EnemyManager>();
        }

        private void OnAnimatorMove()
        {
            float delta = Time.deltaTime;

            enemy.RB.drag = 0;
            Vector3 deltaPosition = enemy.animator.deltaPosition;
            deltaPosition.y = 0;
            Vector3 velocity = deltaPosition / delta;
            enemy.RB.velocity = velocity;
        }
    }
}
