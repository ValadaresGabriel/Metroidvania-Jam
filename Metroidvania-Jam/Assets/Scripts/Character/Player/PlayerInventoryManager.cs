using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class PlayerInventoryManager : CharacterInventoryManager
    {
        public WeaponItem currentRightHandWeapon;
        public WeaponItem currentLeftHandWeapon;

        [Header("Inventory Slots")]
        [SerializeField] private List<Item> items = new();

        [Header("Quick Slots")]
        [SerializeField] private WeaponItem[] weaponsInRightHandSlot = new WeaponItem[3];
        private int rightHandWeaponIndex = 0;

        public void AddItem(Item newItem)
        {
            items.Add(newItem);
        }

        public WeaponItem[] GetWeaponsInRightHandSlot() => weaponsInRightHandSlot;
        public int GetRightHandWeaponIndex() => rightHandWeaponIndex;
        public void UpdateRightHandWeaponIndex(int newWeaponIndex) => rightHandWeaponIndex = newWeaponIndex;
    }
}
