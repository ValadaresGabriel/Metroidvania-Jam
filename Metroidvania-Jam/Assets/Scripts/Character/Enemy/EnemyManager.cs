using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace IM
{
    public class EnemyManager : CharacterManager
    {
        private EnemyLocomotionManager enemyLocomotionManager;

        [HideInInspector]
        public EnemyAnimatorManager enemyAnimatorManager;

        [HideInInspector]
        public NavMeshAgent navMeshAgent;

        [HideInInspector]
        public EnemyStatsManager enemyStatsManager;

        [Header("Target")]

        public CharacterStatsManager currentTarget;

        [SerializeField]
        private State currentState;

        [Header("Locomotion & Ranges Settings")]

        [SerializeField]
        private float rotationSpeed = 15f;

        [SerializeField]
        private float maximumAttackRange = 1.5f;

        [Header("A.I. Settings")]

        [SerializeField]
        private float detectionRadius = 20f;

        [SerializeField]
        private float minimumDetectionAngle = -50f;

        [SerializeField]
        private float maximumDetectionAngle = 50f;

        public float currentRecoveryTime = 0;

        protected override void Awake()
        {
            base.Awake();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
        }

        private void Start()
        {
            navMeshAgent.enabled = false;
            RB.isKinematic = false;
        }

        protected override void Update()
        {
            base.Update();
            HandleRecoveryTime();
        }

        private void FixedUpdate()
        {
            HandleStateMachine();
        }

        private void HandleStateMachine()
        {
            if (currentState != null)
            {
                State nextState = currentState.Tick(this, enemyStatsManager, enemyAnimatorManager);

                if (nextState != null)
                {
                    SwitchToNextState(nextState);
                }
            }
        }

        private void SwitchToNextState(State nextState)
        {
            currentState = nextState;
        }

        // Attacks
        private void HandleRecoveryTime()
        {
            if (currentRecoveryTime > 0)
            {
                currentRecoveryTime -= Time.deltaTime;
            }

            if (isPerformingAction)
            {
                if (currentRecoveryTime <= 0)
                {
                    isPerformingAction = false;
                }
            }
        }

        public float GetDetectionRadius() => detectionRadius;

        public float GetMinimumDetectionAngle() => minimumDetectionAngle;

        public float GetMaximumDetectionAngle() => maximumDetectionAngle;

        public float GetRotationSpeed() => rotationSpeed;

        public float GetMaximumAttackRange() => maximumAttackRange;

    }
}
