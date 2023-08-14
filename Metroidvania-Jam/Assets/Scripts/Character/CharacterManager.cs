using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class CharacterManager : MonoBehaviour
    {
        [HideInInspector]
        public Rigidbody RB { get; private set; }

        [HideInInspector]
        public Animator animator;

        [Header("FLAGS")]
        public bool isPerformingAction = false;
        public bool applyRootMotion = false;
        public bool canRotate = true;
        public bool canMove = true;

        protected virtual void Awake()
        {
            DontDestroyOnLoad(gameObject);

            RB = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        protected virtual void Update()
        {
            //
        }

        protected virtual void LateUpdate()
        {
            //
        }
    }
}
