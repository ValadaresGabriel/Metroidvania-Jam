using UnityEngine;
using TMPro;

namespace TS
{
    public class ControlButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI controlButtonText;

        public void SetControlButtonText(string text)
        {
            controlButtonText.SetText(text);
        }
    }
}
