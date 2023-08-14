using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector]
        public PlayerAnimatorManager playerAnimatorManager;

        [HideInInspector]
        public PlayerLocomotionManager playerLocomotionManager;

        [HideInInspector]
        public PlayerStatsManager playerStatsManager;

        [Header("FLAGS")]

        public bool isSprinting = false;

        protected override void Awake()
        {
            base.Awake();

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            playerStatsManager = GetComponent<PlayerStatsManager>();

            PlayerInputManager.Instance.player = this;
        }

        private void Start()
        {
            // playerNetworkManager.maxStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            //     playerNetworkManager.currentStamina.Value = playerStatsManager.CalculateStaminaBasedOnEnduranceLevel(playerNetworkManager.endurance.Value);
            PlayerUIManager.Instace.playerHUDManager.SetMaxHealthValue(playerStatsManager.GetMaxHealth());
            PlayerUIManager.Instace.playerHUDManager.SetMaxStaminaValue(playerStatsManager.GetMaxStamina());
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
