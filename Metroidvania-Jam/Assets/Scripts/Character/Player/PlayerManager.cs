using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TS
{
    public class PlayerManager : CharacterManager
    {
        [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
        [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
        [HideInInspector] public PlayerInventoryManager playerInventoryManager;
        [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;
        [HideInInspector] public PlayerInteractableManager interactableManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;

        [Header("Player Name")]
        [SerializeField] private string characterName;

        [Header("FLAGS")]
        public bool isSprinting = false;
        public bool isLockedOnEnemy = false;
        public bool isGrounded = true;
        public bool isUsingRightHand = true;
        public bool isUsingLeftHand = false;

        private float secondsPlayed = 0f;

        protected override void Awake()
        {
            base.Awake();

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            characterStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            interactableManager = GetComponent<PlayerInteractableManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();

            PlayerCamera.Instance.player = this;
            PlayerInputManager.Instance.player = this;
            WorldSaveGameManager.Instance.player = this;
        }

        protected override void Update()
        {
            base.Update();

            playerLocomotionManager.HandleAllMovement();

            interactableManager.CheckForInteractableObject(this);

            CountPlayedTime();

            // Regen stamina
            // playerStatsManager.RegenerateStamina();
        }

        private void CountPlayedTime()
        {
            secondsPlayed += Time.deltaTime;
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

            #region Items
            currentCharacterData.currentRightHandWeaponID = playerInventoryManager.currentRightHandWeapon.itemID;
            currentCharacterData.currentLeftHandWeaponID = playerInventoryManager.currentLeftHandWeapon.itemID;
            #endregion

            currentCharacterData.secondsPlayed = secondsPlayed;
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

            #region Items
            playerInventoryManager.currentRightHandWeapon = WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.currentRightHandWeaponID);
            playerInventoryManager.currentLeftHandWeapon = WorldItemDatabase.Instance.GetWeaponByID(currentCharacterData.currentLeftHandWeaponID);
            // playerEquipmentManager.LoadWeaponsOnBothHands();
            playerEquipmentManager.LoadRightWeapon();
            #endregion

            secondsPlayed = currentCharacterData.secondsPlayed;
        }
        #endregion

        #region Weapon And Actions
        public void OnCurrentRightHandWeaponIDChange(int newWeaponID)
        {
            WeaponItem newWeaponInstance = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newWeaponID));
            playerInventoryManager.currentRightHandWeapon = newWeaponInstance;
            playerEquipmentManager.LoadRightWeapon();
        }

        public void OnCurrentWeaponBeingUsedIDChange(int newWeaponID)
        {
            WeaponItem newWeaponInstance = Instantiate(WorldItemDatabase.Instance.GetWeaponByID(newWeaponID));
            playerCombatManager.currentWeaponBeingUsed = newWeaponInstance;
        }

        public void PerformWeaponBasedAction(int actionID, int weaponID)
        {
            WeaponItemAction weaponAction = WorldActionManager.Instance.GetWeaponItemActionByID(actionID);

            if (weaponAction != null)
            {
                weaponAction.AttemptToPerformAction(this, WorldItemDatabase.Instance.GetWeaponByID(weaponID));
            }
            else
            {
                Debug.LogError("ACTION IS NULL, CANNOT BE PERFORMED");
            }
        }
        #endregion
    }
}
