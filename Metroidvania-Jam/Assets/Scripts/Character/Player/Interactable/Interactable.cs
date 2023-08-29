using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IM
{
    public class Interactable : MonoBehaviour
    {
        [SerializeField] private float radius = 0.6f;
        public string interactableText;

        public virtual void Interact(PlayerManager playerManager)
        {
            Debug.Log("Successfull Interacted!");
        }
    }
}
