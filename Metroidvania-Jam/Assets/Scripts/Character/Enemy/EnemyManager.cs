using System.Collections;
using System.Collections.Generic;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.AI;

namespace TS
{
    public class EnemyManager : CharacterManager
    {
        [HideInInspector] public EnemyAnimatorManager enemyAnimatorManager;
        [HideInInspector] public EnemyCombatManager enemyCombatManager;
        [HideInInspector] public NavMeshAgent navMeshAgent;
        [HideInInspector] public EnemyStatsManager enemyStatsManager;
        [HideInInspector] public Rigidbody RB;

        [Header("Target")]
        [SerializeField] private State currentState;
        public CharacterStatsManager currentTarget;

        [Header("Locomotion & Ranges Settings")]
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float maximumAttackRange = 1.5f;

        [Header("A.I. Settings")]
        [SerializeField] private float detectionRadius = 20f;
        [SerializeField] private float minimumDetectionAngle = -50f;
        [SerializeField] private float maximumDetectionAngle = 50f;
        public float currentRecoveryTime = 0;
        public float distanceFromTarget;
        public bool allowAIToPerformCombo;
        public float comboLikelyHood = 0.2f;
        public bool IsInteracting;

        protected override void Awake()
        {
            base.Awake();
            enemyCombatManager = GetComponent<EnemyCombatManager>();
            enemyAnimatorManager = GetComponent<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
            enemyStatsManager = GetComponent<EnemyStatsManager>();
            RB = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            navMeshAgent.enabled = false;
            RB.isKinematic = false;
        }

        protected override void Update()
        {
            base.Update();

            IsInteracting = animator.GetBool("IsInteracting");
            HandleRecoveryTime();
            HandleStateMachine();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            navMeshAgent.transform.position = Vector3.zero;
            navMeshAgent.transform.localRotation = Quaternion.identity;
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
