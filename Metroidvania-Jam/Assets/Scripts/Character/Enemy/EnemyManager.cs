using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class EnemyManager : CharacterManager
    {
        private EnemyLocomotionManager enemyLocomotionManager;

        [Header("A.I. Settings")]

        [SerializeField]
        public float detectionRadius = 20f;

        [SerializeField]
        public float minimumDetectionAngle = -50f;

        [SerializeField]
        public float maximumDetectionAngle = 50f;

        protected override void Awake()
        {
            base.Awake();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
        }

        protected override void Update()
        {
            base.Update();
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotionManager.currentTarget == null)
            {
                enemyLocomotionManager.HandleDetection();
            }
        }
    }
}
