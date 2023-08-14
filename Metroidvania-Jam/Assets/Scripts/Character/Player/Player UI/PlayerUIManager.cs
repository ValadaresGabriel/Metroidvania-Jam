using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class PlayerUIManager : MonoBehaviour
    {
        public static PlayerUIManager Instace { get; private set; }

        [HideInInspector]
        public PlayerHUDManager playerHUDManager;

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
        }
    }
}
