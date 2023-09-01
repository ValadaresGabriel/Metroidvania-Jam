using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class CharacterCombatManager : MonoBehaviour
    {
        [SerializeField]
        protected LayerMask targetLayer;

        [SerializeField]
        protected float detectionRadius;

        [SerializeField]
        private Collider characterCollider;

        protected virtual void Start()
        {
            // characterCollider = GetComponent<Collider>();
        }

        private void OnDrawGizmosSelected()
        {
            if (characterCollider == null) return;

            Gizmos.DrawWireSphere(characterCollider.bounds.center, detectionRadius);
        }
    }
}
