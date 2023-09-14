using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    [System.Serializable]
    public class CharacterSaveData
    {
        [Header("Scene Index")]
        public int sceneIndex = 1;

        [Header("Character Name")]
        public string characterName = "Arthur";

        [Header("Time Player")]
        public float secondsPlayed;

        [Header("World Coordinates")]
        public float xPosition;
        public float yPosition;
        public float zPosition;

        [Header("Player Stats")]
        public float maxHealth = 10;
        public float currentHealth = 10;
        public float maxStamina = 20;
        public float currentStamina = 20;

        [Header("Player Current Items")]
        public int currentRightHandWeaponID;
        public int currentLeftHandWeaponID;

        [Header("Items Looted From World")]
        public SerializableDictionary<int, bool> itemsInWorld; // The int is the world item id, the bool is if the item has been looted.

        [Header("Has Met NPC")]
        public SerializableDictionary<int, bool> npcsInWorld; // The int is the world npc id, the bool is if the player has met the npc before, so he doesn't play the same dialog as the first one.

        public CharacterSaveData()
        {
            itemsInWorld = new SerializableDictionary<int, bool>();
            npcsInWorld = new SerializableDictionary<int, bool>();
        }
    }
}
