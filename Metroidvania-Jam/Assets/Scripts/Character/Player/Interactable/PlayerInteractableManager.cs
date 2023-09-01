using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TS
{
    public class PlayerInteractableManager : MonoBehaviour
    {
        private bool interact = false;

        public void AttemptToInteract()
        {
            interact = true;
        }

        public void CheckForInteractableObject(PlayerManager player)
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, 0.3f, transform.forward, out hit, 1f))
            {
                if (hit.collider.CompareTag("Interactable"))
                {
                    if (hit.collider.TryGetComponent(out Interactable interactableObject))
                    {
                        string interactableText = interactableObject.interactableText;
                        PlayerUIManager.Instace.playerUIPopupManager.SendInteractPopup(interactableText);

                        if (interact)
                        {
                            interact = false;
                            hit.collider.GetComponent<Interactable>().Interact(player);
                        }
                    }
                    else
                    {
                        Debug.LogWarning("COULDN'T FIND/GET INTERACTABLE COMPONENT!");
                    }
                }
            }
            else
            {
                PlayerUIManager.Instace.playerUIPopupManager.CloseInteractPopup();
            }
        }
    }
}
