using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private MeleeWeaponDamageCollider meleeDamageCollider;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterCausingDamage, WeaponItem weapon)
        {
            meleeDamageCollider.characterCausingDamage = characterCausingDamage;
            meleeDamageCollider.SetDamage(weapon.physicalDamage);
        }
    }
}
