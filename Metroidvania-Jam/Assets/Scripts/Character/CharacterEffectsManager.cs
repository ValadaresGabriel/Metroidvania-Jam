using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace TS
{
    public class CharacterEffectsManager : MonoBehaviour
    {
        private CharacterManager characterManager;

        [Header("VFX")]
        [SerializeField] private GameObject bloodSplatterVFX;

        protected virtual void Awake()
        {
            characterManager = GetComponent<CharacterManager>();
        }

        public void ProcessInstantEffect(InstantCharacterEffect effect)
        {
            effect.ProcessEffect(characterManager);
        }

        public void PlayBloodSplatterVFX(Vector3 contactPoint)
        {
            GameObject bloodSplatterInstance;

            // If we manually have placed a blood splatter vfx on this model, play its version
            if (bloodSplatterVFX != null)
            {
                bloodSplatterInstance = Instantiate(bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
            // else, use the generic we have elsewhere
            else
            {
                bloodSplatterInstance = Instantiate(WorldCharacterEffectsManager.Instance.bloodSplatterVFX, contactPoint, Quaternion.identity);
            }
        }
    }
}
