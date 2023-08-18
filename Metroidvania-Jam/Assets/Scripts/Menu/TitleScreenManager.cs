using UnityEngine;

namespace IM
{
    public class TitleScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject optionPanel;
        public void StartNewGame()
        {
            StartCoroutine(GameManager.Instance.LoadNewGame());
        }

        public void ExitGame()
        {
            Application.Quit();
        }

        public void OpenOption()
        {
            optionPanel.SetActive(true);
        }

        public void CloseOption()
        {
            optionPanel.SetActive(false);
        }
    }
}