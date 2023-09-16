using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace TS
{
    public class PlayerUIPauseManager : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuGameObject;
        [SerializeField] private Button resumeButton;

        public void InitializePause()
        {
            pauseMenuGameObject.SetActive(true);
            resumeButton.Select();
            Time.timeScale = 0f;
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            pauseMenuGameObject.SetActive(false);
            PlayerInputManager.Instance.isMenuOpened = false;
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
