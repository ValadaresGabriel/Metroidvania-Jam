using System.Collections;
using System.Collections.Generic;
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
        [HideInInspector] public EnemyInventoryManager enemyInventoryManager;
        [HideInInspector] public EnemyEquipmentManager enemyEquipmentManager;
        [HideInInspector] public Rigidbody RB;

        [Header("Target")]
        [SerializeField] private State currentState;
        public CharacterStatsManager currentTarget;

        [Header("Locomotion & Ranges Settings")]
        [SerializeField] private float rotationSpeed = 15f;
        [SerializeField] private float maximumAttackRange = 2f;

        [Header("A.I. Settings")]
        [SerializeField] private float detectionRadius = 15f;
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
            enemyEquipmentManager = GetComponent<EnemyEquipmentManager>();
            enemyInventoryManager = GetComponent<EnemyInventoryManager>();
            RB = GetComponent<Rigidbody>();
        }

        protected override void Start()
        {
            base.Start();

            navMeshAgent.enabled = false;
            RB.isKinematic = false;
            OnCurrentWeaponBeingUsedIDChange(0);
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
            if (isDead) return;

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

        public void OnCurrentRightHandWeaponIDChange(int newWeaponID)
        {
            WeaponItem newWeaponInstance = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newWeaponID));
            enemyInventoryManager.currentRightHandWeapon = newWeaponInstance;
            enemyEquipmentManager.LoadRightWeapon();
        }

        public void OnCurrentWeaponBeingUsedIDChange(int newWeaponID)
        {
            WeaponItem newWeaponInstance = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newWeaponID));
            enemyCombatManager.currentWeaponBeingUsed = newWeaponInstance;
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
