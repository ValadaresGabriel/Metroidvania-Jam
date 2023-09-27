using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class MeleeWeaponDamageCollider : DamageCollider
    {
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
            base.OnTriggerEnter(other);
        }

        protected override void DamageTarget(CharacterManager damageTarget)
        {
            base.DamageTarget(damageTarget);
        }
    }
}
