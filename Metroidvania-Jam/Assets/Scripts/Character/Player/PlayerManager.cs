using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IM
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;

        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;

        [HideInInspector] public PlayerInventoryManager playerInventoryManager;

        [Header("Player Name")]
        [SerializeField] private string characterName;

        [Header("FLAGS")]
        public bool isSprinting = false;
        public bool isLockedOnEnemy = false;

        protected override void Awake()
        {
            base.Awake();

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            characterStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();

            PlayerCamera.Instance.player = this;
            PlayerInputManager.Instance.player = this;
            WorldSaveGameManager.Instance.player = this;
        }

        private void Start()
        {
            PlayerUIManager.Instace.playerHUDManager.SetMaxHealthValue(characterStatsManager.GetMaxHealth());
            PlayerUIManager.Instace.playerHUDManager.SetMaxStaminaValue(characterStatsManager.GetMaxStamina());
        }

        public override void UpdateCharacterHealth(float newHealthValue)
        {
            base.UpdateCharacterHealth(newHealthValue);

            PlayerUIManager.Instace.playerHUDManager.SetNewHealthValue(characterStatsManager.GetCurrentHealth());
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            PlayerUIManager.Instace.playerUIPopupManager.SendYouDiedPopup();
            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }

        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            characterStatsManager.InitializeStats();
            playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
            isDead = false;
        }

        protected override void Update()
        {
            base.Update();

            playerLocomotionManager.HandleAllMovement();

            // Regen stamina
            // playerStatsManager.RegenerateStamina();
        }

        protected override void LateUpdate()
        {
            base.LateUpdate();

            PlayerCamera.Instance.HandleAllCameraActions();
        }

        #region Save System
        public void SaveGameDataToCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            currentCharacterData.sceneIndex = SceneManager.GetActiveScene().buildIndex;
            currentCharacterData.characterName = characterName;
            currentCharacterData.xPosition = transform.position.x;
            currentCharacterData.yPosition = transform.position.y;
            currentCharacterData.zPosition = transform.position.z;
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            characterName = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;
        }
        #endregion
    }
}
