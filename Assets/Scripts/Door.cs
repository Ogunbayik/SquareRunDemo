using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    private Animator animator;

    [Header("Settings")]
    [SerializeField] private int doorID;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Key.OnPlayerCollectedKey += Key_OnPlayerCollectedKey;
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
    }
    public void CloseDoor()
    {
        animator.SetTrigger(Consts.DoorAnimationParameter.CLOSING_PARAMETER);
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
