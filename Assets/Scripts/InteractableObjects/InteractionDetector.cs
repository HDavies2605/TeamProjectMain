using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null; // Reference to the interactable object currently in range
    public GameObject interactionIcon;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        interactionIcon.SetActive(false); // Hide the interaction icon at the start
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            interactableInRange?.Interact(); // If there is an interactable object in range, call its Interact method
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())   //check if the object we collided with has an IInteractable component
        {
            interactableInRange = interactable; // Store reference to the interactable object
            interactionIcon.SetActive(true); // Show the interaction icon
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)   //checking if player is no longer in range
        {
            interactableInRange = null;
            interactionIcon.SetActive(false); // hide the interaction icon
        }
    }
}
