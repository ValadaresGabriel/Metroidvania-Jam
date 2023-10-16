using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class WorldUICameraManager : MonoBehaviour
    {
        public WorldUICameraManager Instance { get; private set; }

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
    }
}
