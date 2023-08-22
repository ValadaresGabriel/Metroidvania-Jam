using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        private CharacterManager characterManager;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(characterManager);
        }
    }
}
