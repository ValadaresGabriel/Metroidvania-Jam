using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    [CreateAssetMenu(menuName = "Character Effects/Instant Effects/Take Damage")]
    public class TakeDamageEffect : InstantCharacterEffect
    {
        [Header("Character Causing Damage")]
        [SerializeField] public CharacterManager characterCausingDamage;

        [Header("Animation")]
        [SerializeField] private bool playDamageAnimation = true;
        [SerializeField] private bool manuallySelectDamageAnimation = false;
        [SerializeField] private string damageAnimation;
        [SerializeField] public float damage;

        [Header("Sound FX")]
        [SerializeField] private bool playDamageSFX = true;
        [SerializeField] private AudioClip elementalDamageSound;

        [Header("Direction Damage Taken From")]
        [SerializeField] private float angleHitDirection;
        [SerializeField] private Vector3 contactPoint;

        public override void ProcessEffect(CharacterManager characterTakingDamage)
        {
            if (characterCausingDamage != null)
            {
                if (characterCausingDamage.isDead) return;
                // Check for damages
            }

            if (characterTakingDamage.isDead) return;

            damage = Mathf.Clamp(damage, 1f, damage);

            CalculateHealthDamage(characterTakingDamage);
        }

        public void CalculateHealthDamage(CharacterManager characterTakingDamage)
        {
            characterTakingDamage.UpdateCharacterHealth(damage);
        }

        public void SetDamage(float newDamage) => damage = newDamage;
    }
}
