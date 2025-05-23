using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInteraction : MonoBehaviour
{
    public static event Action<PlayerInteraction,IHitable> OnPlayerHitted;
    public static event Action<int> OnClosedDoor;

    [Header("Interact Settings")]
    [SerializeField] private KeyCode interactButton;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ICollectable>(out ICollectable collectable))
        {
            collectable.Collect(this);
        }

        if(other.gameObject.TryGetComponent<PhaseStartTrigger>(out PhaseStartTrigger phaseStartTrigger))
        {
            
            GameManager.StartNewPhase();
            ClosedTheDoor(phaseStartTrigger);
            Destroy(phaseStartTrigger.gameObject);
        }

        if(other.gameObject.TryGetComponent<IHitable>(out IHitable hitable))
        {
            OnPlayerHitted?.Invoke(this, hitable);
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

    public void ClosedTheDoor(PhaseStartTrigger phaseTrigger)
    {
        OnClosedDoor?.Invoke(phaseTrigger.GetDoorID());
    }
}
