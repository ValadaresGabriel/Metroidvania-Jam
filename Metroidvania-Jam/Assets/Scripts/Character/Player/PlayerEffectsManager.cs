using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class PlayerEffectsManager : CharacterEffectsManager
    {
        [SerializeField]
        private InstantCharacterEffect effectToTest;

        [SerializeField]
        private bool processEffect = false;
        private void Update()
        {
            if (processEffect)
            {
                processEffect = false;

                InstantCharacterEffect effectInstance = Instantiate(effectToTest);
                ProcessInstantEffect(effectInstance);
            }
        }
    }
}
