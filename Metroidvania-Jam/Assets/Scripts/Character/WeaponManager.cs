using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class WeaponManager : MonoBehaviour
    {
        public MeleeWeaponDamageCollider meleeDamageCollider;

        private void Awake()
        {
            meleeDamageCollider = GetComponentInChildren<MeleeWeaponDamageCollider>();
        }

        public void SetWeaponDamage(CharacterManager characterCausingDamage, WeaponItem weapon)
        {
            meleeDamageCollider.characterCausingDamage = characterCausingDamage;
            meleeDamageCollider.light_attack_01_modifier = weapon.light_attack_01_modifier;
            meleeDamageCollider.SetDamage(weapon.physicalDamage);
        }
    }
}
