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
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerInteractableManager interactableManager;

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
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            interactableManager = GetComponent<PlayerInteractableManager>();

            PlayerCamera.Instance.player = this;
            PlayerInputManager.Instance.player = this;
            WorldSaveGameManager.Instance.player = this;
        }

        protected override void Update()
        {
            base.Update();

            playerLocomotionManager.HandleAllMovement();

            interactableManager.CheckForInteractableObject(this);

            // Regen stamina
            // playerStatsManager.RegenerateStamina();
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

        [ContextMenu("Revive Character")]
        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            characterStatsManager.InitializeStats();
            playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
            isDead = false;
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

            #region Stats
            currentCharacterData.maxHealth = characterStatsManager.GetMaxHealth();
            currentCharacterData.currentHealth = characterStatsManager.GetCurrentHealth();

            currentCharacterData.maxStamina = characterStatsManager.GetMaxStamina();
            currentCharacterData.currentStamina = characterStatsManager.GetCurrentStamina();
            #endregion
        }

        public void LoadGameDataFromCurrentCharacterData(ref CharacterSaveData currentCharacterData)
        {
            characterName = currentCharacterData.characterName;
            Vector3 myPosition = new Vector3(currentCharacterData.xPosition, currentCharacterData.yPosition, currentCharacterData.zPosition);
            transform.position = myPosition;

            #region Stats
            characterStatsManager.SetMaxHealth(currentCharacterData.maxHealth);
            characterStatsManager.SetMaxStamina(currentCharacterData.maxStamina);

            characterStatsManager.SetCurrentHealth(currentCharacterData.currentHealth);
            characterStatsManager.SetCurrentStamina(currentCharacterData.currentStamina);

            PlayerUIManager.Instace.playerHUDManager.SetMaxHealthValue(characterStatsManager.GetMaxHealth());
            PlayerUIManager.Instace.playerHUDManager.SetMaxStaminaValue(characterStatsManager.GetMaxStamina());

            PlayerUIManager.Instace.playerHUDManager.SetNewHealthValue(characterStatsManager.GetCurrentHealth());
            PlayerUIManager.Instace.playerHUDManager.SetNewStaminaValue(characterStatsManager.GetCurrentStamina());
            #endregion
        }
        #endregion

        #region Change Weapon
        public void OnCurrentRightHandWeaponIDChange(int newWeaponID)
        {
            WeaponItem newWeaponInstance = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newWeaponID));
            playerInventoryManager.currentRightHandWeapon = newWeaponInstance;
            playerEquipmentManager.LoadRightWeapon();
        }
        #endregion
    }
}
