using System.Collections;
using System.Collections.Generic;
using IM;
using UnityEngine;
using UnityEngine.UI;

namespace RPG
{
    public class UI_StatBar : MonoBehaviour
    {
        private Slider slider;

        private RectTransform rectTransform;
        // Variable to scale bar size depending on stat (higher stat = longer bar across screen)
        // Secondary bar behind may bar for polish effect (yellow bar that shows how much an action/damage takes away from current stat)

        [Header("Bar Options")]
        [SerializeField]
        protected bool scaleBarLengthWithStats = true;

        [SerializeField]
        protected float widthScaleMultiplier = 1f;

        protected virtual void Awake()
        {
            if (slider == null)
            {
                slider = GetComponent<Slider>();
            }
        }

        public virtual void SetStat(float newValue)
        {
            slider.value = newValue;
        }

        public virtual void SetMaxStat(float maxValue)
        {
            slider.maxValue = maxValue;
            slider.value = maxValue;

            if (scaleBarLengthWithStats)
            {
                rectTransform.sizeDelta = new Vector2(maxValue * widthScaleMultiplier, rectTransform.sizeDelta.y);
                PlayerUIManager.Instace.playerHUDManager.RefreshHUD();
            }
        }
    }
}
