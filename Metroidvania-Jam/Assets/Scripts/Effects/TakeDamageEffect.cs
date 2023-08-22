using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]

        [SerializeField]
        public CharacterManager characterCausingDamage;

        [Header("Animation")]

        [SerializeField]
        private bool playDamageAnimation = true;

        [SerializeField]
        private bool manuallySelectDamageAnimation = false;

        [SerializeField]
        private string damageAnimation;

        [SerializeField]
        private float damage;

        [Header("Sound FX")]

        [SerializeField]
        private bool playDamageSFX = true;

        [SerializeField]
        private AudioClip elementalDamageSound;

        [Header("Direction Damage Taken From")]

        [SerializeField]
        private float angleHitDirection;

        [SerializeField]
        private Vector3 contactPoint;

        public override void ProcessEffect(CharacterManager characterManager)
        {
            CalculateHealthDamage(characterManager);

            if (characterManager.isDead) return;

            if (characterCausingDamage != null)
            {
                // Check for damages
            }

            if (damage <= 0)
            {
                damage = 1;
            }
        }

        public void CalculateHealthDamage(CharacterManager characterManager)
        {
            characterManager.UpdateCharacterHealth(damage);
        }

        public void SetDamage(float newDamage) => damage = newDamage;
    }
}
