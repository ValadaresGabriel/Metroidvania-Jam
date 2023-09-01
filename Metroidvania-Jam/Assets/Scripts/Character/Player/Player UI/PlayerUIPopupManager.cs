using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace TS
{
    public class PlayerUIPopupManager : MonoBehaviour
    {
        [Header("You Died Pop Up")]
        [SerializeField] private GameObject youDiedPopupGameObject;
        [SerializeField] private TextMeshProUGUI youDiedPopupBackgroundText;
        [SerializeField] private TextMeshProUGUI youDiedPopupText;
        [SerializeField] private CanvasGroup youDiedPopupCanvasGroup;

        [Header("Interact Popup")]
        [SerializeField] private GameObject interactPopUpGameObject;
        [SerializeField] private TextMeshProUGUI interactText;

        #region You Died
        public void SendYouDiedPopup()
        {
            youDiedPopupGameObject.SetActive(true);
            youDiedPopupBackgroundText.characterSpacing = 0;

            StartCoroutine(StretchPopUpTextOverTime(youDiedPopupBackgroundText, 8, 19));
            StartCoroutine(FadeInPopUpOverTime(youDiedPopupCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopupCanvasGroup, 2, 5));
        }

        private IEnumerator StretchPopUpTextOverTime(TextMeshProUGUI text, float duration, float stretchAmount)
        {
            if (duration > 0f)
            {
                text.characterSpacing = 0;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    text.characterSpacing = Mathf.Lerp(text.characterSpacing, stretchAmount, duration * (Time.deltaTime / 20));
                    yield return null;
                }
            }

            yield return null;
        }

        private IEnumerator FadeInPopUpOverTime(CanvasGroup canvasGroup, float duration)
        {
            if (duration > 0f)
            {
                canvasGroup.alpha = 0;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 1, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvasGroup.alpha = 1;

            yield return null;
        }

        private IEnumerator WaitThenFadeOutPopUpOverTime(CanvasGroup canvasGroup, float duration, float delay)
        {
            if (duration > 0f)
            {
                while (delay > 0)
                {
                    delay -= Time.deltaTime;
                    yield return null;
                }

                canvasGroup.alpha = 1;
                float timer = 0;

                yield return null;

                while (timer < duration)
                {
                    timer += Time.deltaTime;
                    canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, 0, duration * Time.deltaTime);
                    yield return null;
                }
            }

            canvasGroup.alpha = 0;

            yield return null;
        }
        #endregion

        #region Interact
        public void SendInteractPopup(string newInteractText)
        {
            interactPopUpGameObject.SetActive(true);
            interactText.text = newInteractText;
        }

        public void CloseInteractPopup()
        {
            if (interactPopUpGameObject.activeSelf)
            {
                interactPopUpGameObject.SetActive(false);
            }
        }
        #endregion
    }
}
