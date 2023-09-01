using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class PlayerHUDManager : MonoBehaviour
    {
        [SerializeField]
        private UI_StatBar healthBar;

        [SerializeField]
        private UI_StatBar staminaBar;

        public void RefreshHUD()
        {
            healthBar.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(true);

            staminaBar.gameObject.SetActive(false);
            staminaBar.gameObject.SetActive(true);
        }

        #region Health
        public void SetNewHealthValue(float newValue)
        {
            healthBar.SetStat(newValue);
        }

        public void SetMaxHealthValue(float maxHealth)
        {
            healthBar.SetMaxStat(maxHealth);
        }
        #endregion

        #region Stamina
        public void SetNewStaminaValue(float newValue)
        {
            staminaBar.SetStat(newValue);
        }

        public void SetMaxStaminaValue(float maxStamina)
        {
            staminaBar.SetMaxStat(maxStamina);
        }
        #endregion
    }
}
