using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private float radius = 0.6f;
        public string interactableText;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }

        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("Successfull Interacted!");
        }
    }
}
