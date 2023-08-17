using UnityEngine;

namespace IM
{
    public class TitleScreenManager : MonoBehaviour
    {
        public void StartNewGame()
        {
            StartCoroutine(GameManager.Instance.LoadNewGame());
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}