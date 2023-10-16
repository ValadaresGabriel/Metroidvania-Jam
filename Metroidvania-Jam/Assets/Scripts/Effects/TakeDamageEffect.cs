using System;
using System.Collections;
using System.Collections.Generic;
using EZCameraShake;
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

        private float damage;

        [Header("Final Damage")]
        private float finalDamageDealt = 0;

        [Header("Poise")]
        public float poiseDamage = 0;
        public bool poiseIsBroken = false;

        [Header("Sound FX")]
        [SerializeField] private bool playDamageSFX = true;
        [SerializeField] private AudioClip elementalDamageSound;

        [Header("Direction Damage Taken From")]
        [SerializeField] private float angleHitFrom;
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

            // Check which directional damage came from
            PlayDirectionalBasedDamageAnimation(characterTakingDamage);
            // Check for build ups (poison, bleed etc)
            PlayDamageSFX(characterTakingDamage);
            PlayDamageVFX(characterTakingDamage);

            CameraShaker.Instance.ShakeOnce(1.5f, 1.5f, 0f, .8f);
            CombatUtilityManager.Instance.PlaySleepTimeWhenHit();
            // If character is A.I, check for new target if character causing damage is present
        }

        private void CalculateHealthDamage(CharacterManager characterTakingDamage)
        {
            characterTakingDamage.UpdateCharacterHealth(damage);
        }

        private void PlayDamageVFX(CharacterManager character)
        {
            // If we have fire damage, play fire particles
            // Lightning damage, lightning partciles

            character.characterEffectsManager.PlayBloodSplatterVFX(contactPoint);
        }

        private void PlayDamageSFX(CharacterManager character)
        {
            // If fire damage > 0, play fire sfx
            AudioClip physicalDamageSFX = WorldSoundFXManager.Instance.ChooseRandomSFXFromArray(WorldSoundFXManager.Instance.physicalDamageSFX);

            character.characterSoundFXManager.PlaySoundFX(physicalDamageSFX, .7f);
        }

        private void PlayDirectionalBasedDamageAnimation(CharacterManager character)
        {
            if (character.isDead) return;

            // TO DO: Calculate if poise is broken
            poiseIsBroken = true;

            if (angleHitFrom >= 145 && angleHitFrom <= 180)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.Forward_Medium_Damages);
            }
            else if (angleHitFrom <= -145 && angleHitFrom >= -180)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.Forward_Medium_Damages);
            }
            else if (angleHitFrom >= -45 && angleHitFrom <= 45)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.Backward_Medium_Damages);
            }
            else if (angleHitFrom >= -144 && angleHitFrom <= -45)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.Left_Medium_Damages);
            }
            else if (angleHitFrom >= 45 && angleHitFrom <= 144)
            {
                damageAnimation = character.characterAnimatorManager.GetRandomAnimationFromList(character.characterAnimatorManager.Right_Medium_Damages);
            }

            // If poise is broken, play a staggering damage animation
            if (poiseIsBroken)
            {
                character.characterAnimatorManager.LastDamageAnimationPlayed = damageAnimation;
                character.characterAnimatorManager.PlayTargetActionAnimation(damageAnimation, true);
            }
        }

        public float Damage
        {
            get => damage;
            set => damage = value;
        }

        public float AngleHitFrom
        {
            get => angleHitFrom;
            set => angleHitFrom = value;
        }

        public Vector3 ContactPoint
        {
            get => contactPoint;
            set => contactPoint = value;
        }
    }
}
