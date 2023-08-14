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

        private void Start()
        {
            InitializeStats();
        }

        public void InitializeStats()
        {
            SetCurrentHealth(maxHealth);
            SetCurrentStamina(maxStamina);
        }

        public float GetMaxHealth() => maxHealth;

        public float GetMaxStamina() => maxStamina;

        public float GetCurrentHealth() => currentHealth;

        public float GetCurrentStamina() => currentStamina;

        public void SetCurrentHealth(float newHealth) => currentHealth = newHealth;

        public void SetCurrentStamina(float newStamina) => currentStamina = newStamina;
    }
}
