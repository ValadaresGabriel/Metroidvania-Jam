using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace TS
{
    public class PlayerUIPopupManager : MonoBehaviour
    {
        [Header("Control Button Game Object")]
        [SerializeField] private GameObject controlGameObject;

        [Header("You Died Pop Up")]
        [SerializeField] private GameObject youDiedPopupGameObject;
        [SerializeField] private TextMeshProUGUI youDiedPopupBackgroundText;
        [SerializeField] private TextMeshProUGUI youDiedPopupText;
        [SerializeField] private CanvasGroup youDiedPopupCanvasGroup;

        [Header("Interact Popup")]
        [SerializeField] private GameObject interactPopUpGameObject;
        [SerializeField] private GameObject interactPopUpControlButtonGameObject;
        [SerializeField] private TextMeshProUGUI interactText;

        [Header("Interact Response Popup")]
        [SerializeField] private GameObject interactResponsePopUpGameObject;
        [SerializeField] private TextMeshProUGUI interactResponseMessage;
        [SerializeField] private Button closeInteractResponseButton;

        private GameObject controlInstance;

        #region You Died
        public void SendYouDiedPopup()
        {
            youDiedPopupGameObject.SetActive(true);
            youDiedPopupBackgroundText.characterSpacing = 0;

            StartCoroutine(StretchPopUpTextOverTime(youDiedPopupBackgroundText, 8, 19));
            StartCoroutine(FadeInPopUpOverTime(youDiedPopupCanvasGroup, 5));
            StartCoroutine(WaitThenFadeOutPopUpOverTime(youDiedPopupCanvasGroup, 2, 5));
            CloseInteractPopup();
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
        public bool IsInteractPopupOpen()
        {
            return interactPopUpGameObject.activeSelf;
        }

        public void SendInteractPopup(Interactable interactableObject)
        {
            string newInteractText = interactableObject.interactableText;

            if (interactPopUpGameObject.activeSelf && interactText.text == newInteractText) return;

            if (interactableObject.control != null && interactableObject.control != "")
            {
                controlInstance = Instantiate(controlGameObject, interactPopUpControlButtonGameObject.transform.position, Quaternion.identity);
                controlInstance.GetComponent<ControlButton>().SetControlButtonText(interactableObject.control);
                controlInstance.transform.SetParent(interactPopUpControlButtonGameObject.transform);
                controlInstance.transform.localScale = new Vector3(1, 1, 1);
            }

            interactPopUpGameObject.SetActive(true);
            interactText.text = newInteractText;
        }

        public void CloseInteractPopup()
        {
            if (interactPopUpGameObject.activeSelf)
            {
                Destroy(controlInstance);
                interactPopUpGameObject.SetActive(false);
            }
        }
        #endregion

        #region Interact Response
        public void InitializeInteractResponseMessage(string newText)
        {
            closeInteractResponseButton.Select();
            interactResponsePopUpGameObject.SetActive(true);
            SetInteractResponseMessage(newText);
        }

        public void SetInteractResponseMessage(string newText)
        {
            interactResponseMessage.SetText(newText);
        }

        public void CloseInteractResponseMessage()
        {
            interactResponsePopUpGameObject.SetActive(false);
        }
        #endregion
    }
}
