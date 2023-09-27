using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class WorldCursorManager : MonoBehaviour
    {
        public static WorldCursorManager Instance { get; private set; }

        public bool IsCursorVisible { get; private set; }

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
            HideCursor();
        }

        public void HideCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            IsCursorVisible = false;
        }

        public void ShowCursor()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            IsCursorVisible = true;
        }
    }
}
