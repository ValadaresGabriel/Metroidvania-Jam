using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TS
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

        private string FormatSecondsToTime(int totalSeconds)
        {
            // Converter o total de segundos em horas, minutos e segundos
            int hours = totalSeconds / 3600;
            int remainingSeconds = totalSeconds % 3600;
            int minutes = remainingSeconds / 60;
            int seconds = remainingSeconds % 60;

            // Formatar para que fique com dois dígitos
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

            return formattedTime;
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
                        timedPlayed.text = FormatSecondsToTime(Mathf.RoundToInt(WorldSaveGameManager.Instance.characterSlot01.secondsPlayed));
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
                        timedPlayed.text = FormatSecondsToTime(Mathf.RoundToInt(WorldSaveGameManager.Instance.characterSlot02.secondsPlayed));
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
                        timedPlayed.text = FormatSecondsToTime(Mathf.RoundToInt(WorldSaveGameManager.Instance.characterSlot03.secondsPlayed));
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
