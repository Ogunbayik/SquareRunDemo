using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStateManager : MonoBehaviour
{
    public static PlayerStateManager Instance;

    public static Action<PlayerStates, PlayerStates> OnStateChanged;

    public enum PlayerStates
    {
        Idle,
        Walking,
        Running,
    }

    public PlayerStates currentState;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        currentState = PlayerStates.Idle;
    }

    public void ChangeState(PlayerStates newState)
    {
        var previousState = currentState;

        if (previousState == newState)
            return;

        currentState = newState;
        OnStateChanged?.Invoke(previousState, currentState);

        Debug.Log("Player State is changed " + previousState + " to " + currentState);
    }

    public PlayerStates GetCurrentState()
    {
        return currentState;
    }
}
