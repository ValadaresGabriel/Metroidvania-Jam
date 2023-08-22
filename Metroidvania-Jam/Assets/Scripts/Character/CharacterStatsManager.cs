using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

namespace IM
{
    public class CharacterStatsManager : MonoBehaviour
    {
        [SerializeField]
        protected float maxHealth;

        [SerializeField]
        protected float currentHealth;

        [SerializeField]
        protected float maxStamina;

        [SerializeField]
        protected float currentStamina;

        protected virtual void Start()
        {
            InitializeStats();
        }

        public virtual void InitializeStats()
        {
            SetCurrentHealth(maxHealth);
            SetCurrentStamina(maxStamina);
        }

        public float GetMaxHealth() => maxHealth;

        public float GetMaxStamina() => maxStamina;

        public float GetCurrentHealth() => currentHealth;

        public float GetCurrentStamina() => currentStamina;

        public virtual void SetMaxHealth(float newMaxHealth) => maxHealth = newMaxHealth;

        public virtual void SetMaxStamina(float newMaxStamina) => maxStamina = newMaxStamina;

        public void SetCurrentHealth(float newHealth)
        {
            currentHealth = newHealth;

            if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else if (currentHealth < 0)
            {
                currentHealth = 0;
            }
        }

        public void SetCurrentStamina(float newStamina) => currentStamina = newStamina;
    }
}
