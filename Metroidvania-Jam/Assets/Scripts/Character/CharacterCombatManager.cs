using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class CharacterCombatManager : MonoBehaviour
    {
        public AttackType currentAttackType;
        private CharacterManager character;

        protected virtual void Awake()
        {
            character = GetComponent<CharacterManager>();
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
