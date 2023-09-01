using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TS
{
    public class WorldCharacterEffectsManager : MonoBehaviour
    {
        public static WorldCharacterEffectsManager Instance { get; private set; }

        [Header("Damage")]
        public TakeDamageEffect takeDamageEffect;

        [SerializeField]
        private List<InstantCharacterEffect> instantEffects;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            GenerateEffectsIDs();
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void GenerateEffectsIDs()
        {
            for (int i = 0; i < instantEffects.Count; i++)
            {
                instantEffects[i].instantEffectID = i;
            }
        }
    }
}
