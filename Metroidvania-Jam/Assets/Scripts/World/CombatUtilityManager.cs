using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class CombatUtilityManager : MonoBehaviour
    {
        public static CombatUtilityManager Instance { get; private set; }

        [Header("Sleep Time when Hit")]
        [SerializeField] private float newTimeScaleTimeWhenHit = .25f;
        [SerializeField] private float timeToSleepTimeWhenHit = .25f;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void PlaySleepTimeWhenHit()
        {
            StartCoroutine(StartSleepTime());
        }

        private IEnumerator StartSleepTime()
        {
            Time.timeScale = newTimeScaleTimeWhenHit;

            yield return new WaitForSeconds(timeToSleepTimeWhenHit);

            Time.timeScale = 1;
        }
    }
}
