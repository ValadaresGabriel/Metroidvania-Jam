using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace IM
{
    public class UI_Character_Save_Slot : MonoBehaviour
    {
        private SaveFileDataWriter saveFileWriter;

        [Header("Game Slot")]
        public CharacterSlot characterSlot;

        [Header("Character Info")]
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI timedPlayed;

        private void OnEnable()
        {
            LoadSaveSlot();
        }

        private void LoadSaveSlot()
        {
            saveFileWriter = new SaveFileDataWriter
            {
                saveDataDirectoyPath = Application.persistentDataPath,
                saveFileName = WorldSaveGameManager.Instance.DecideCharacterFileNameBasedOnCharacterSlotBeingUsed(characterSlot)
            };

            switch (characterSlot)
            {
                case CharacterSlot.CharacterSlot_01:
                    if (saveFileWriter.FileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot01.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_02:
                    if (saveFileWriter.FileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot02.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case CharacterSlot.CharacterSlot_03:
                    if (saveFileWriter.FileExists())
                    {
                        characterName.text = WorldSaveGameManager.Instance.characterSlot03.characterName;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public void LoadGameFromCharacterSlot()
        {
            WorldSaveGameManager.Instance.currentCharacterSlotBeingUsed = characterSlot;
            WorldSaveGameManager.Instance.LoadGame();
        }

        public void SelectCurrentSlot()
        {
            TitleScreenManager.Instance.SelectCharacterSlot(characterSlot);
        }
    }
}
