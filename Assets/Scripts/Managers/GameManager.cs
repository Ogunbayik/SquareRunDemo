using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static Action OnTeleport;

    public enum GameStates
    {
        GameStart,
        InGame,
        Teleporting,
        GameOver,
        PassedPhase,
        GameWin
    }

    public static GameManager Instance { get; private set; }

    public enum GamePhase
    {
        FirstPhase,
        SecondPhase,
        ThirdPhase,
        LastPhase
    }

    public GamePhase currentPhase;
    public GameStates currentState;

    [SerializeField] private float maxDelayTime;

    public int currentPhaseIndex;

    private Vector3 playerMovementDirection;

    private float horizontalInput;
    private float verticalInput;
    private float delayTimer;

    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion
    }
    void Start()
    {
        currentPhase = GamePhase.FirstPhase;
        currentState = GameStates.GameStart;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case GameStates.GameStart:
                SetPlayerMovementDirection();

                break;
            case GameStates.InGame:
                SetPlayerMovementDirection();


                delayTimer = maxDelayTime;
                //Playing and This state can change GameOver or Waiting
                break;
            case GameStates.Teleporting:
                playerMovementDirection = Vector3.zero;

                delayTimer -= Time.deltaTime;

                if(delayTimer <= 0)
                {
                    OnTeleport?.Invoke();
                    delayTimer = maxDelayTime;
                }
                break;
            case GameStates.GameOver:
                break;
            case GameStates.PassedPhase:
                break;
            case GameStates.GameWin:
                break;
        }
    }
    private void SetPlayerMovementDirection()
    {
        horizontalInput = Input.GetAxisRaw(Consts.InputConts.HORIZONTAL_INPUT);
        verticalInput = Input.GetAxisRaw(Consts.InputConts.VERTICAL_INPUT);

        switch (currentPhaseIndex)
        {
            case 0:
                currentPhase = GamePhase.FirstPhase;
                playerMovementDirection = new Vector3(horizontalInput, 0f, verticalInput);
                break;
            case 1:
                currentPhase = GamePhase.SecondPhase;
                playerMovementDirection = new Vector3(verticalInput, 0f, -horizontalInput);
                break;
            case 2:
                currentPhase = GamePhase.ThirdPhase;
                playerMovementDirection = new Vector3(-horizontalInput, 0f, -verticalInput);
                break;
            case 3:
                currentPhase = GamePhase.LastPhase;
                playerMovementDirection = new Vector3(-verticalInput, 0f, horizontalInput);
                break;
        }
    }


    public Vector3 GetMovementDirectionNormalized()
    {
        return playerMovementDirection.normalized;
    }

    public void ChangeState(GameStates state)
    {
        if (currentState == state)
            return;

        currentState = state;
    }

    public void ResetDelayTimer()
    {
        delayTimer = maxDelayTime;
    }
}
