using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    [System.Serializable]
    public struct TerrainSounds
    {
        public AudioClip[] walk;
        public AudioClip[] run;
        public AudioClip[] startJump;
        public AudioClip[] landJump;
    }

    public class WorldSoundFXManager : MonoBehaviour
    {
        public static WorldSoundFXManager Instance { get; private set; }

        [Header("Damage Sounds")]
        public AudioClip[] physicalDamageSFX;

        [Header("Action Sounds")]
        public AudioClip rollSFX;

        [Header("Character Ambient Interaction Sound")]
        [Header("Grass")]
        public TerrainSounds grassSounds;

        [Header("Dirt")]
        public TerrainSounds dirtSounds;

        [Header("Rock & Road")]
        public TerrainSounds rockRoadSounds;

        [Header("Sand")]
        public TerrainSounds sandSounds;

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
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public AudioClip ChooseRandomSFXFromArray(AudioClip[] array)
        {
            int index = Random.Range(0, array.Length);

            return array[index];
        }
    }
}
