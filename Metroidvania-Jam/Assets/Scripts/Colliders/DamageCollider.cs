using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class DamageCollider : MonoBehaviour
    {
        [Header("Damage")]
        [SerializeField]
        private float damage;

        [Header("Contact Point")]
        private Vector3 contactPoint;

        [Header("Characters Damaged")]
        private List<CharacterManager> charactersDamaged = new List<CharacterManager>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterManager damageTarget))
            {
                Debug.Log("Entered");
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
            damageEffect.SetDamage(damage);

            damageTarget.characterEffectsManager.ProcessInstantEffect(damageEffect);
        }
    }
}
