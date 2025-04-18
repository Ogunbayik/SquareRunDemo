using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interact Settings")]
    [SerializeField] private KeyCode interactButton;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ICollectable>(out ICollectable collectable))
        {
            collectable.Collect(this);
        }
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
