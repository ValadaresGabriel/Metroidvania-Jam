using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector]
        public PlayerAnimatorManager playerAnimatorManager;

        public bool isSprinting = false;

        [HideInInspector]
        public PlayerLocomotionManager playerLocomotionManager;

        protected override void Awake()
        {
            base.Awake();

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            PlayerInputManager.Instance.player = this;
        }

        protected override void Update()
        {
            base.Update();

            playerLocomotionManager.HandleAllMovement();

            // Regen stamina
            // playerStatsManager.RegenerateStamina();
        }
    }
}
