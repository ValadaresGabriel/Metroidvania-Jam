using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Instance { get; private set; }

        [HideInInspector] public PlayerHUDManager playerHUDManager;
        [HideInInspector] public PlayerUIPopupManager playerUIPopupManager;
        [HideInInspector] public PlayerUIDialogManager playerUIDialogManager;
        [HideInInspector] public PlayerUIPauseManager playerUIPauseManager;
        [HideInInspector] public WorldObjectiveManager worldObjectiveManager;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            playerHUDManager = GetComponentInChildren<PlayerHUDManager>();
            playerUIPopupManager = GetComponentInChildren<PlayerUIPopupManager>();
            playerUIDialogManager = GetComponentInChildren<PlayerUIDialogManager>();
            playerUIPauseManager = GetComponentInChildren<PlayerUIPauseManager>();
            worldObjectiveManager = GetComponentInChildren<WorldObjectiveManager>();
        }
    }
}
