using UnityEngine;

namespace IM
{
    public class TitleScreenManager : MonoBehaviour
    {
        [SerializeField] private GameObject optionPanel;
        [SerializeField] private GameObject playerPrefab;
        public void StartNewGame()
        {
            Instantiate(playerPrefab);
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