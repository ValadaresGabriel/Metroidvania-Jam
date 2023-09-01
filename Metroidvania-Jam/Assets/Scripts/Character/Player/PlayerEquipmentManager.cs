using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class PlayerEquipmentManager : CharacterEquipmentManager
    {
        private PlayerManager playerManager;

        [Header("Weapon Slots")]
        public WeaponModelInstantiationSlot rightHandSlot;
        public WeaponModelInstantiationSlot leftHandSlot;

        [Header("Weapon Manager")]
        [SerializeField] private WeaponManager rightWeaponManager;
        [SerializeField] private WeaponManager leftWeaponManager;

        [Header("Weapon Models")]
        public GameObject rightHandWeaponModel;
        public GameObject leftHandWeaponModel;

        protected override void Awake()
        {
            base.Awake();

            playerManager = GetComponent<PlayerManager>();

            InitializeWeaponSlots();
        }

        protected override void Start()
        {
            base.Start();

            LoadWeaponsOnBothHands();
        }

        private void InitializeWeaponSlots()
        {
            WeaponModelInstantiationSlot[] weaponSlots = GetComponentsInChildren<WeaponModelInstantiationSlot>();

            foreach (WeaponModelInstantiationSlot weaponSlot in weaponSlots)
            {
                if (weaponSlot.weaponSlot == WeaponModelSlot.RightHand)
                {
                    rightHandSlot = weaponSlot;
                }
                else if (weaponSlot.weaponSlot == WeaponModelSlot.LeftHand)
                {
                    leftHandSlot = weaponSlot;
                }
            }
        }

        public void LoadWeaponsOnBothHands()
        {
            LoadRightWeapon();
            LoadLeftWeapon();
        }

        public void LoadRightWeapon()
        {
            if (playerManager.playerInventoryManager.currentRightHandWeapon != null)
            {
                rightHandSlot.UnloadWeaponModel();
                rightHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(playerManager, playerManager.playerInventoryManager.currentRightHandWeapon);
            }
        }

        [ContextMenu("Switch Weapon")]
        public void SwitchRightHandWeapon()
        {
            PlayWeaponSwapAnimation();

            UpdateRightHandWeaponIndex();
        }

        private void PlayWeaponSwapAnimation()
        {
            playerManager.playerAnimatorManager.PlayTargetActionAnimation("Swap_Weapon", false, true, true, true);
        }

        private void UpdateRightHandWeaponIndex()
        {
            // Caching frequent method calls
            int currentIndex = playerManager.playerInventoryManager.GetRightHandWeaponIndex();
            WeaponItem[] rightHandWeapons = playerManager.playerInventoryManager.GetWeaponsInRightHandSlot();
            int weaponCount = rightHandWeapons.Length;

            // Calculate new index and update
            int newIndex = (currentIndex + 1) % (weaponCount + 1);
            playerManager.playerInventoryManager.UpdateRightHandWeaponIndex(newIndex);

            if (newIndex < weaponCount)
            {
                // Update if a weapon exists at the new index
                WeaponItem selectedWeapon = rightHandWeapons[newIndex];
                playerManager.OnCurrentRightHandWeaponIDChange(selectedWeapon.itemID);
            }
            else if (newIndex == weaponCount)
            {
                // Handle the case where no weapon exists in the new slot (reset or do something else)
                SwitchRightHandWeapon();
            }
            else
            {
                // Handle unexpected cases. For instance, when newIndex is out of bounds.
                // Reset to the first weapon
                playerManager.playerInventoryManager.UpdateRightHandWeaponIndex(0);
                playerManager.OnCurrentRightHandWeaponIDChange(rightHandWeapons[0]?.itemID ?? 0);
            }
        }


        public void LoadLeftWeapon()
        {
            if (playerManager.playerInventoryManager.currentLeftHandWeapon != null)
            {
                leftHandWeaponModel = Instantiate(playerManager.playerInventoryManager.currentLeftHandWeapon.weaponModel);
                leftHandSlot.LoadWeapon(leftHandWeaponModel);
                leftWeaponManager = leftHandWeaponModel.GetComponent<WeaponManager>();
                leftWeaponManager.SetWeaponDamage(playerManager, playerManager.playerInventoryManager.currentLeftHandWeapon);
            }
        }
    }
}
