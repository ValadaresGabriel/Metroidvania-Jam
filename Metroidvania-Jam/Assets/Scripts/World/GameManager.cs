using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public bool isSprinting;

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

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

    }
}
