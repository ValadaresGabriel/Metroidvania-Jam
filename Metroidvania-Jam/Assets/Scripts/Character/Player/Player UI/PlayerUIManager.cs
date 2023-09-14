using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Instace { get; private set; }

        [HideInInspector] public PlayerHUDManager playerHUDManager;
        [HideInInspector] public PlayerUIPopupManager playerUIPopupManager;
        [HideInInspector] public PlayerUIDialogManager playerUIDialogManager;
        [HideInInspector] public PlayerUIPauseManager playerUIPauseManager;

        private void Awake()
        {
            if (Instace == null)
            {
                Instace = this;
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
        }
    }
}
