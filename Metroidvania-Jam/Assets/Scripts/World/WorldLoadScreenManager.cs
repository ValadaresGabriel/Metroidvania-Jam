using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TS
{
    public class WorldLoadScreenManager : MonoBehaviour
    {
        public static WorldLoadScreenManager Instance { get; private set; }

        [SerializeField] private Animator animator;
        [SerializeField] private GameObject canvas;

        private float time = 1;

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

        public void LoadScreenManagement(bool active)
        {
            if (!active)
            {
                animator.SetTrigger("FadeOut");

                while (time > 0)
                {
                    time -= Time.deltaTime;
                }

                time = 1;
            }

            canvas.SetActive(active);
        }
    }
}
