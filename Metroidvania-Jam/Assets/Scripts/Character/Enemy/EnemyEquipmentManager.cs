using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class EnemyEquipmentManager : CharacterEquipmentManager
    {
        private EnemyManager enemyManager;

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

            enemyManager = GetComponent<EnemyManager>();

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
            // LoadLeftWeapon();
        }

        #region Right Weapon
        public void LoadRightWeapon()
        {
            if (enemyManager.enemyInventoryManager.currentRightHandWeapon != null)
            {
                rightHandSlot.UnloadWeaponModel();
                rightHandWeaponModel = Instantiate(enemyManager.enemyInventoryManager.currentRightHandWeapon.weaponModel);
                rightHandSlot.LoadWeapon(rightHandWeaponModel);
                rightWeaponManager = rightHandWeaponModel.GetComponent<WeaponManager>();
                rightWeaponManager.SetWeaponDamage(enemyManager, enemyManager.enemyInventoryManager.currentRightHandWeapon);
            }
        }
        #endregion

        #region Damage Colliders
        public void OpenDamageCollider()
        {
            // Open right hand
            rightWeaponManager.meleeDamageCollider.EnableDamageCollider();

            // TO DO: open left hand

            // Play woosh SFX
        }

        public void CloseDamageCollider()
        {
            // Open right hand
            rightWeaponManager.meleeDamageCollider.DisableDamageCollider();

            // TO DO: open left hand
        }
        #endregion
    }
}
