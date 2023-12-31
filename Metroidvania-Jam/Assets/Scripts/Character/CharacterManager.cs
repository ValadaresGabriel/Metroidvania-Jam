using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class CharacterManager : MonoBehaviour
    {
        // [HideInInspector]
        // public Rigidbody RB { get; private set; }
        [HideInInspector] public CharacterController characterController;
        [HideInInspector] public Animator animator;
        [HideInInspector] public CharacterStatsManager characterStatsManager;
        [HideInInspector] public CharacterEffectsManager characterEffectsManager;
        [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
        [HideInInspector] public CharacterCombatManager characterCombatManager;
        [HideInInspector] public CharacterSoundFXManager characterSoundFXManager;
        [HideInInspector] public CharacterLocomotionManager characterLocomotionManager;

        [Header("Character Status")]
        public bool isDead = false;

        [Header("Flags")]
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;
        public bool canDoCombo;
        public bool isJumping = false;
        public bool isGrounded = false;
        public bool isChargingAttack = false;
        public bool isChargeAttackReleased = false;
        public bool isChargeAttackFullReleased = false;

        [Header("Colliders")]
        [SerializeField] private Collider parentCollider;
        [SerializeField] private Collider characterCollisionBlocker;

        protected virtual void Awake()
        {
            // DontDestroyOnLoad(gameObject);

            // RB = GetComponent<Rigidbody>();
            characterController = GetComponent<CharacterController>();
            animator = GetComponent<Animator>();
            characterStatsManager = GetComponent<CharacterStatsManager>();
            characterEffectsManager = GetComponent<CharacterEffectsManager>();
            characterAnimatorManager = GetComponent<CharacterAnimatorManager>();
            characterCombatManager = GetComponent<CharacterCombatManager>();
            characterSoundFXManager = GetComponent<CharacterSoundFXManager>();
            characterLocomotionManager = GetComponent<CharacterLocomotionManager>();

            Physics.IgnoreCollision(parentCollider, characterCollisionBlocker, true);
        }

        protected virtual void Start()
        {

        }

        public virtual void UpdateCharacterHealth(float newHealthValue)
        {
            characterStatsManager.SetCurrentHealth(characterStatsManager.GetCurrentHealth() - newHealthValue);

            if (characterStatsManager.GetCurrentHealth() <= 0)
            {
                StartCoroutine(ProcessDeathEvent());
            }
        }

        protected virtual void Update()
        {
            HandleAnimatorFlags();
        }

        private void HandleAnimatorFlags()
        {
            canDoCombo = animator.GetBool("CanDoCombo");
            animator.SetBool("IsGrounded", isGrounded);
            animator.SetBool("IsChargeAttackReleased", isChargeAttackReleased);
            animator.SetBool("IsChargeAttackFullReleased", isChargeAttackFullReleased);
        }

        protected virtual void LateUpdate()
        {
            //
        }

        public virtual IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            characterStatsManager.SetCurrentHealth(0);
            isDead = true;

            if (!manuallySelectDeathAnimation)
            {
                characterAnimatorManager.PlayTargetActionAnimation("Death 01", true);
            }

            // Play death SFX

            yield return new WaitForSeconds(5);

            // Award with something

            // disable character
        }

        public virtual void ReviveCharacter()
        {

        }
    }
}
