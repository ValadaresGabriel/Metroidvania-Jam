using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class WorldUtilityManager : MonoBehaviour
    {
        public static WorldUtilityManager Instance { get; private set; }

        [Header("Layers")]
        [SerializeField] private LayerMask characterLayers;
        [SerializeField] private LayerMask enviroLayers;

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

        public LayerMask GetCharacterLayers => characterLayers;
        public LayerMask GetEnviroLayers => enviroLayers;
    }
}
