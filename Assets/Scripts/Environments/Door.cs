using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    private BoxCollider doorCollider;

    [Header("Settings")]
    [SerializeField] private int doorID;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        doorCollider = GetComponent<BoxCollider>();
    }
    private void OnEnable()
    {
        Key.OnPlayerCollectedKey += Key_OnPlayerCollectedKey;
        PlayerInteraction.OnClosedDoor += PlayerInteraction_OnClosedDoor;
    }

    private void PlayerInteraction_OnClosedDoor(int triggerID)
    {
        if (triggerID == doorID)
            CloseDoor();
    }

    private void OnDisable()
    {
        Key.OnPlayerCollectedKey -= Key_OnPlayerCollectedKey;
    }
    private void Key_OnPlayerCollectedKey(Key key)
    {
        var keyID = key.GetKeyID();

        if (keyID == doorID)
            OpenDoor();
    }
    public void OpenDoor()
    {
        animator.SetTrigger(Consts.DoorAnimationParameter.OPENING_PARAMETER);
        doorCollider.enabled = false;
    }
    public void CloseDoor()
    {
        animator.SetTrigger(Consts.DoorAnimationParameter.CLOSING_PARAMETER);
        doorCollider.enabled = true;
        //Change Gamemanager state for ingame
    }

    public void OpenIdle()
    {
        animator.SetTrigger(Consts.DoorAnimationParameter.OPENED_PARAMETER);
    }

    public void Idle()
    {
        animator.SetTrigger(Consts.DoorAnimationParameter.CLOSED_PARAMETER);
    }

    public int GetDoorID()
    {
        return doorID;
    }
}
