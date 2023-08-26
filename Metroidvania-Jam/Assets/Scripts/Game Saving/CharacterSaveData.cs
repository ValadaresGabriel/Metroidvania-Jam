using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex = 1;

        [Header("Character Name")]
        public string characterName = "[Character Name]";

        [Header("Time Player")]
        public float secondsPlayed;

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;
    }
}
