using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private KeyCode interactButton;


    private void Start()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            if(Input.GetKey(interactButton))
            {
                interactable.Interact(this);
            }

        }
    }
}
