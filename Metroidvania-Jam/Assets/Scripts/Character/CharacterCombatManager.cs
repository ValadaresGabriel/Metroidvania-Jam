using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class CharacterCombatManager : MonoBehaviour
    {
        [Header("Attack Target")]
        public CharacterManager currentTarget;

        [Header("Attack Type")]
        public AttackType currentAttackType;

        [Header("Lock On Transform")]
        public Transform lockOnTransform;

        private CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
        }

        public virtual void SetTarget(CharacterManager newTarget)
        {
            if (newTarget != null)
            {
                currentTarget = newTarget;
            }
            else
            {
                currentTarget = null;
            }
        }

        public virtual void OpenCanDoCombo()
        {
            character.animator.SetBool("CanDoCombo", true);
        }

        public virtual void CloseCanDoCombo()
        {
            character.animator.SetBool("CanDoCombo", false);
        }
    }
}
