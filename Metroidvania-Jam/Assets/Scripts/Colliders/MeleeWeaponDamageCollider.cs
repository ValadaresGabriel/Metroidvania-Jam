using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;

        [Header("Weapon Attack Modifiers")]
        public float light_attack_01_modifier;

        protected override void Awake()
        {
            base.Awake();

            if (damageCollider == null)
            {
                damageCollider = GetComponent<Collider>();
            }

            damageCollider.enabled = false;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterManager damageTarget))
            {
                // Do not deal damage to ourself
                if (damageTarget == characterCausingDamage) return;

                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // Check if we can damage target (friendly)
                // Check if target is blocking, somehow
                // Check if target is vulnerable

                DamageTarget(damageTarget);
            }
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            // Can't deal damage more than once per attack on the same target
            // So, we add them to a list that checks before applying damage

            if (charactersDamaged.Contains(damageTarget)) return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
            damageEffect.SetDamage(damage);

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_attack_01_modifier, damageEffect);
                    break;
                default:
                    break;
            }

            Debug.Log(damageTarget);
            Debug.Log(damageEffect);

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        private void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.damage *= modifier;

            // If attack is a fully charged heavy, multiply by full charge modifier after normal modifier have been calculated
        }
    }
}
