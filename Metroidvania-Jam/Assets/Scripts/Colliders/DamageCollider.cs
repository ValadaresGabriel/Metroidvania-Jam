using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Attacking Character")]
        public CharacterManager characterCausingDamage;

        [Header("Collider")]
        protected Collider damageCollider;

        [Header("Damage")]
        [SerializeField] protected float damage;

        [Header("Contact Point")]
        protected Vector3 contactPoint;

        [Header("Characters Damaged")]
        protected List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        [Header("Weapon Light Attack Modifiers")]
        public float light_attack_01_modifier;
        public float light_attack_02_modifier;
        public float light_attack_03_modifier;

        [Header("Weapon Heavy Attack Modifiers")]
        public float heavy_attack_01_modifier;
        public float heavy_attack_02_modifier;
        public float heavy_attack_03_modifier;

        protected virtual void Awake()
        {

        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterManager damageTarget))
            {
                contactPoint = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);

                // Check if we can damage target (friendly)
                // Check if target is blocking, somehow
                // Check if target is vulnerable

                DamageTarget(damageTarget);
            }
        }

        protected virtual void DamageTarget(CharacterManager damageTarget)
        {
            // Can't deal damage more than once per attack on the same target
            // So, we add them to a list that checks before applying damage

            if (charactersDamaged.Contains(damageTarget)) return;

            charactersDamaged.Add(damageTarget);

            TakeDamageEffect damageEffect = Instantiate(WorldCharacterEffectsManager.Instance.takeDamageEffect);
            damageEffect.characterCausingDamage = characterCausingDamage;
            damageEffect.Damage = damage;
            damageEffect.ContactPoint = contactPoint;
            damageEffect.AngleHitFrom = Vector3.SignedAngle(characterCausingDamage.transform.forward, damageTarget.transform.forward, Vector3.up);

            switch (characterCausingDamage.characterCombatManager.currentAttackType)
            {
                case AttackType.LightAttack01:
                    ApplyAttackDamageModifiers(light_attack_01_modifier, damageEffect);
                    break;
                case AttackType.LightAttack02:
                    ApplyAttackDamageModifiers(light_attack_02_modifier, damageEffect);
                    break;
                case AttackType.LightAttack03:
                    ApplyAttackDamageModifiers(light_attack_03_modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack01:
                    ApplyAttackDamageModifiers(heavy_attack_01_modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack02:
                    ApplyAttackDamageModifiers(heavy_attack_02_modifier, damageEffect);
                    break;
                case AttackType.HeavyAttack03:
                    ApplyAttackDamageModifiers(heavy_attack_03_modifier, damageEffect);
                    break;
                default:
                    break;
            }

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }

        protected virtual void ApplyAttackDamageModifiers(float modifier, TakeDamageEffect damage)
        {
            damage.Damage *= modifier;

            // If attack is a fully charged heavy, multiply by full charge modifier after normal modifier have been calculated
        }

        public virtual void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public virtual void DisableDamageCollider()
        {
            damageCollider.enabled = false;
            charactersDamaged.Clear();
        }

        public void SetDamage(float newDamage) => damage = newDamage;
    }
}
