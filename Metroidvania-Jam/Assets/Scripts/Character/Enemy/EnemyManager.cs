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
        public EnemyAnimatorManager animatorManager;

        [HideInInspector]
        public NavMeshAgent navMeshAgent;

        [Header("A.I. Settings")]

        [SerializeField]
        public float detectionRadius = 20f;

        [SerializeField]
        public float minimumDetectionAngle = -50f;

        [SerializeField]
        public float maximumDetectionAngle = 50f;

        [Header("Enemy Attacks")]

        [SerializeField]
        private EnemyAttackAction[] enemyAttacks;

        [SerializeField]
        private EnemyAttackAction currentAttack;

        public float currentRecoveryTime = 0;

        protected override void Awake()
        {
            base.Awake();
            enemyLocomotionManager = GetComponent<EnemyLocomotionManager>();
            animatorManager = GetComponent<EnemyAnimatorManager>();
            navMeshAgent = GetComponentInChildren<NavMeshAgent>();
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
            HandleCurrentAction();
        }

        private void HandleCurrentAction()
        {
            if (enemyLocomotionManager.currentTarget != null)
            {
                enemyLocomotionManager.SetDistanceFromTarget(Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position));
            }

            if (enemyLocomotionManager.currentTarget == null)
            {
                enemyLocomotionManager.HandleDetection();
            }
            else if (enemyLocomotionManager.GetDistanceFromTarget() > enemyLocomotionManager.GetStoppingDistance())
            {
                enemyLocomotionManager.HandleMoveToTarget();
            }
            else if (enemyLocomotionManager.GetDistanceFromTarget() <= enemyLocomotionManager.GetStoppingDistance())
            {
                GetAttackTarget();
            }
        }

        // Attacks
        private void GetAttackTarget()
        {
            if (isPerformingAction) return;

            if (currentAttack == null)
            {
                GetNewAttack();
            }
            else
            {
                currentRecoveryTime = currentAttack.RecoveryTime;
                animatorManager.PlayTargetActionAnimation(currentAttack.ActionAnimation, true);
                currentAttack = null;
            }
        }

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

        private void GetNewAttack()
        {
            Vector3 targetsDirection = enemyLocomotionManager.currentTarget.transform.position - transform.position;
            float viewableAngle = Vector3.Angle(targetsDirection, transform.forward);

            enemyLocomotionManager.SetDistanceFromTarget(Vector3.Distance(enemyLocomotionManager.currentTarget.transform.position, transform.position));

            int maxScore = 0;

            foreach (EnemyAttackAction attack in enemyAttacks)
            {
                if (enemyLocomotionManager.GetDistanceFromTarget() <= attack.MaximumDistanceNeededToAttack && enemyLocomotionManager.GetDistanceFromTarget() >= attack.MinimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= attack.MaximumAttackAngle && viewableAngle >= attack.MinimumDistanceNeededToAttack)
                    {
                        maxScore += attack.AttackScore;
                    }
                }
            }

            int randomValue = Random.Range(0, maxScore);
            int temporaryScore = 0;

            foreach (EnemyAttackAction attack in enemyAttacks)
            {
                if (enemyLocomotionManager.GetDistanceFromTarget() <= attack.MaximumDistanceNeededToAttack && enemyLocomotionManager.GetDistanceFromTarget() >= attack.MinimumDistanceNeededToAttack)
                {
                    if (viewableAngle <= attack.MaximumAttackAngle && viewableAngle >= attack.MinimumDistanceNeededToAttack)
                    {
                        if (currentAttack != null) return;

                        temporaryScore += attack.AttackScore;

                        if (temporaryScore > randomValue)
                        {
                            currentAttack = attack;
                        }
                    }
                }
            }
        }
    }
}
