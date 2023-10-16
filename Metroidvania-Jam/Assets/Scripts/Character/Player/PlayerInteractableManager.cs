using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TS
{
    public class PlayerInteractableManager : MonoBehaviour
    {
        private bool interact = false;
        [SerializeField] private float interactableArea = 0.4f;
        [SerializeField] private float maxDistance = 3f;

        public void AttemptToInteract()
        {
            interact = true;
        }

        public void CheckForInteractableObject(PlayerManager player)
        {
            RaycastHit hit;

            if (Physics.SphereCast(transform.position, interactableArea, transform.forward, out hit, maxDistance))
            {
                if (PlayerInputManager.Instance.IsInteracting)
                {
                    if (PlayerUIManager.Instance.playerUIPopupManager.IsInteractPopupOpen())
                    {
                        PlayerUIManager.Instance.playerUIPopupManager.CloseInteractPopup();
                    }

                    return;
                }

                if (hit.collider.CompareTag("Interactable"))
                {
                    if (hit.collider.TryGetComponent(out Interactable interactableObject))
                    {
                        PlayerUIManager.Instance.playerUIPopupManager.SendInteractPopup(interactableObject);

                        if (interact)
                        {
                            interact = false;
                            PlayerInputManager.Instance.IsInteracting = true;
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
                PlayerUIManager.Instance.playerUIPopupManager.CloseInteractPopup();
            }
        }
    }
}
