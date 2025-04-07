using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerStates currentState;
    void Start()
    {
        currentState = PlayerStates.Idle;
    }

    public void ChangeState(PlayerStates state)
    {
        if(currentState == state)
        {
            return;
        }

        currentState = state;
    }

    public PlayerStates GetCurrentState()
    {
        return currentState;
    }
}
