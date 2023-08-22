using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class WeaponItem : Item
    {
        // Animator controller override (change attack animations based on weapon you are currently using)

        [Header("Weapon Model")]
        public GameObject weaponModel;

        [Header("Weapon Requirements")]
        public int strREQ = 0;
        public int dexREQ = 0;
        public int intREQ = 0;
        public int faithREQ = 0;

        [Header("Weapon Base Damage")]
        public int physicalDamage = 0;
        public int magicDamage = 0;
        public int fireDamage = 0;
        public int holyDamage = 0;
        public int lightningDamage = 0;

        [Header("Stamina Cost")]
        public int baseStaminaCost = 20;

        // Weapon modifiers
        // Light attack modifiers
        // Heavy attack modifiers
        // Critical Damage modifiers
    }
}
