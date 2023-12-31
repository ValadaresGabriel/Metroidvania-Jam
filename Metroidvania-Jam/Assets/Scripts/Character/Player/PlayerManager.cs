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
        [HideInInspector] public PlayerInteractableManager playerInteractableManager;
        [HideInInspector] public PlayerCombatManager playerCombatManager;

        [Header("Player Name")]
        [SerializeField] private string characterName;

        [Header("Flags")]
        public bool isSprinting = false;
        public bool isLockedOn = false;
        public bool isUsingRightHand = true;
        public bool isUsingLeftHand = false;

        private float secondsPlayed = 0f;

        public bool canUseHeavyAttack = false;

        protected override void Awake()
        {
            base.Awake();

            playerAnimatorManager = GetComponent<PlayerAnimatorManager>();
            playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
            characterStatsManager = GetComponent<PlayerStatsManager>();
            playerInventoryManager = GetComponent<PlayerInventoryManager>();
            playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
            playerInteractableManager = GetComponent<PlayerInteractableManager>();
            playerCombatManager = GetComponent<PlayerCombatManager>();

            PlayerCamera.Instance.player = this;
            PlayerInputManager.Instance.player = this;
            WorldSaveGameManager.Instance.player = this;
        }

        protected override void Start()
        {
            base.Start();

            SceneManager.activeSceneChanged += OnSceneChange;
        }

        protected override void Update()
        {
            base.Update();

            DontDestroyOnLoad(gameObject);

            playerLocomotionManager.HandleAllMovement();

            playerInteractableManager.CheckForInteractableObject(this);

            CountPlayedTime();

            // Regen stamina
            // playerStatsManager.RegenerateStamina();
        }

        private void OnSceneChange(Scene oldScene, Scene newScene)
        {
            characterSoundFXManager.FindTerrain();
        }

        private void CountPlayedTime()
        {
            secondsPlayed += Time.deltaTime;
        }

        public override void UpdateCharacterHealth(float newHealthValue)
        {
            base.UpdateCharacterHealth(newHealthValue);

            PlayerUIManager.Instance.playerHUDManager.SetNewHealthValue(characterStatsManager.GetCurrentHealth());
        }

        public void UpdateCharacterStamina(float newStaminaValue)
        {
            characterStatsManager.SetCurrentHealth(characterStatsManager.GetCurrentStamina() - newStaminaValue);
            PlayerUIManager.Instance.playerHUDManager.SetNewStaminaValue(characterStatsManager.GetCurrentStamina());
        }

        public override IEnumerator ProcessDeathEvent(bool manuallySelectDeathAnimation = false)
        {
            PlayerUIManager.Instance.playerUIPopupManager.SendYouDiedPopup();
            return base.ProcessDeathEvent(manuallySelectDeathAnimation);
        }

        [ContextMenu("Revive Character")]
        public override void ReviveCharacter()
        {
            base.ReviveCharacter();

            isDead = false;
            characterStatsManager.InitializeStats();
            playerAnimatorManager.PlayTargetActionAnimation("Empty", false);
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

            if (currentCharacterData.secondsPlayed < 10)
            {
                Transform spawnPointTransform = GameObject.FindGameObjectWithTag("Spawn Point").transform;
                transform.position = spawnPointTransform.position;
            }

            #region Stats
            characterStatsManager.SetMaxHealth(currentCharacterData.maxHealth);
            characterStatsManager.SetMaxStamina(currentCharacterData.maxStamina);

            characterStatsManager.SetCurrentHealth(currentCharacterData.currentHealth);
            characterStatsManager.SetCurrentStamina(currentCharacterData.currentStamina);

            PlayerUIManager.Instance.playerHUDManager.SetMaxHealthValue(characterStatsManager.GetMaxHealth());
            PlayerUIManager.Instance.playerHUDManager.SetMaxStaminaValue(characterStatsManager.GetMaxStamina());

            PlayerUIManager.Instance.playerHUDManager.SetNewHealthValue(characterStatsManager.GetCurrentHealth());
            PlayerUIManager.Instance.playerHUDManager.SetNewStaminaValue(characterStatsManager.GetCurrentStamina());
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

        private void OnDestroy()
        {
            SceneManager.activeSceneChanged -= OnSceneChange;
        }
    }
}
