using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TS
{
    public class Interactable : MonoBehaviour
    {
        [TextArea] public string interactableText = "Press <b><size=40>E</size></b> or <b><size=40>Y</size></b> to interact";

        [Tooltip("Control Button, if there is any")]
        [SerializeField] public string control;

        protected PlayerManager player;

        protected virtual void Awake()
        {

        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {

        }

        public virtual void Interact(PlayerManager playerManager)
        {
            player = playerManager;
            Debug.Log("Successfull Interacted!");
        }
    }
}
