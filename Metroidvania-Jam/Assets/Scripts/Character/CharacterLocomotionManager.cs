using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

namespace TS
{
    public class CharacterLocomotionManager : MonoBehaviour
    {
        private CharacterManager character;

        [Header("Ground Check & Jumping")]
        [SerializeField] LayerMask groundLayer;
        [SerializeField] protected float gravityForce = -5.55f;
        [SerializeField] private float groundCheckSphereRadius = 1;

        [Tooltip("The force at which our character is pulled up or down (jumping or falling)")]
        [SerializeField] protected Vector3 yVelocity;

        [Tooltip("The force at which our character is sticking to the ground whilst they are grounded")]
        [SerializeField] protected float groundedYVelocity = -20;

        [Tooltip("The force at which our character begins to fall when they become ungrounded (rises as they fall longer)")]
        [SerializeField] protected float fallStartYVelocity = -5;

        protected bool fallingVelocityHasBeenSet = false;
        protected float inAirTimer = 0;

        [Header("Flags")]
        public bool isRolling = false;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        protected virtual void Update()
        {
            HandleGroundCheck();

            if (character.isGrounded)
            {
                // If we are not attempting to jump or move upward
                if (yVelocity.y < 0)
                {
                    inAirTimer = 0;
                    fallingVelocityHasBeenSet = false;
                    yVelocity.y = groundedYVelocity;
                }
            }
            else
            {
                // If we are not jumping and our falling velocity has not been set
                if (!character.isJumping && !fallingVelocityHasBeenSet)
                {
                    fallingVelocityHasBeenSet = true;
                    yVelocity.y = fallStartYVelocity;
                }

                inAirTimer += Time.deltaTime;
                character.animator.SetFloat("InAirTimer", inAirTimer);

                yVelocity.y += gravityForce * Time.deltaTime;
            }

            // There should always be some force applied to the y velocity
            character.characterController.Move(yVelocity * Time.deltaTime);
        }

        protected void HandleGroundCheck()
        {
            character.isGrounded = Physics.CheckSphere(character.transform.position, groundCheckSphereRadius, groundLayer);
        }

    }
}
