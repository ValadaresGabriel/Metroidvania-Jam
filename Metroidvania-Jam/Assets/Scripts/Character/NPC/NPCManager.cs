using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class NPCManager : MonoBehaviour
    {
        private Animator animator;
        [SerializeField] private string[] characterAnimations = { "Idle", "Kneel", "Sitting_01", "Sitting_02", "Sitting_Disbelief", "Sitting_Idle_01", "Sitting_Idle_02", "General_Dead_01" };
        [SerializeField] private int targetAnimation;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            animator.Play(characterAnimations[targetAnimation]);
        }
    }
}
