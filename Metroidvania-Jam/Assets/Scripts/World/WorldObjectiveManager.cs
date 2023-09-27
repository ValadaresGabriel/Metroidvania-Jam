using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

namespace TS
{
    public class WorldObjectiveManager : MonoBehaviour
    {
        public static WorldObjectiveManager Instance { get; private set; }

        [Header("World Objective Configurations")]
        [SerializeField] private List<WorldObjective> worldObjectives;
        [SerializeField] private WorldObjective currentWorldObjective;
        [SerializeField] private AudioSource objectiveAudioSource;
        [SerializeField] private AudioClip takeNewObjectiveSFX;
        [SerializeField] private AudioClip completeCurrentObjectiveSFX;

        [Header("Modal Game Objects")]
        [SerializeField] GameObject modalWorldObjectiveGameObject;
        [SerializeField] Button modalWorldObjectiveMaxMinButton;
        [SerializeField] TextMeshProUGUI modalWorldObjectiveMaxMinButtonText;

        [Header("Modal World Objective UI Content")]
        [SerializeField] private TextMeshProUGUI modalWorldObjectiveTitleText;
        [SerializeField] private TextMeshProUGUI modalWorldObjectiveDescriptionText;

        [Header("Completed Game Objects")]
        [SerializeField] GameObject completedWorldObjectiveGameObject;
        [SerializeField] Button completedWorldObjectiveOkButton;

        [Header("Completed World Objective UI Content")]
        [SerializeField] private TextMeshProUGUI completedWorldObjectiveTitleText;
        [SerializeField] private TextMeshProUGUI completedWorldObjectiveDescriptionText;

        private void Start()
        {
            StartWorldObjective();
        }

        public void StartWorldObjective()
        {
            currentWorldObjective = worldObjectives[WorldSaveGameManager.Instance.currentCharacterData.objectiveIndex];
            modalWorldObjectiveTitleText.text = currentWorldObjective.title;
            modalWorldObjectiveDescriptionText.text = currentWorldObjective.description;
        }

        public void SendWorldObjective()
        {

        }

        public void MaxMinModal()
        {
            modalWorldObjectiveGameObject.SetActive(!modalWorldObjectiveGameObject.activeSelf);
            ChangeButtonIcon();
        }

        private void ChangeButtonIcon()
        {
            if (modalWorldObjectiveGameObject.activeSelf)
            {
                modalWorldObjectiveMaxMinButtonText.text = "^";
                return;
            }

            modalWorldObjectiveMaxMinButtonText.text = "v";
        }

        public void ConfigureCurrentWorldObjective()
        {
            currentWorldObjective = worldObjectives[WorldSaveGameManager.Instance.currentCharacterData.objectiveIndex];
        }

        public void CompleteCurrentObjective()
        {
            // Here we complete the player's current objective
            // Play completeCurrentObjective SFX
            // Show completedWorldObjective
            WorldSaveGameManager.Instance.currentCharacterData.objectiveIndex++;
            currentWorldObjective = worldObjectives[WorldSaveGameManager.Instance.currentCharacterData.objectiveIndex];
        }
    }
}
