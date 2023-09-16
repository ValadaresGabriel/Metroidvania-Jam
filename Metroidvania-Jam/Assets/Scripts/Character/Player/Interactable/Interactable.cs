using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private float radius = 0.6f;
        [TextArea] public string interactableText = "Press <b><size=40>E</size></b> or <b><size=40>Y</size></b> to interact";

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
