using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Timeline;

namespace IM
{
    public class EnemyLocomotionManager : CharacterLocomotionManager
    {
        private EnemyManager enemy;

        protected override void Awake()
        {
            enemy = GetComponent<EnemyManager>();
        }

        protected override void Update()
        {
        }
    }
}
