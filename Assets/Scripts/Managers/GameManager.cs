using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    private PlayerController playerController;

    public static Action OnWinGame;
    public static Action OnGameover;

    public static event Action OnGamePhaseStart;
    public static event Action OnPlayerPassedPhase;
    public static event Action OnGameOverPhase;

    public enum GameStates
    {
        GameStart,
        InGame,
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
    [Header("Game Settings")]
    public GamePhase currentPhase;
    public GameStates currentState;
    [Header("Delay Settings")]
    [SerializeField] private float delayTeleporting;
    [SerializeField] private float delayNextState;
    [Header("Rotation Settings")]
    [SerializeField] private int spikeRotationY;

    private int phaseIndex;

    private float horizontalInput;
    private float verticalInput;

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

        playerController = GameObject.FindObjectOfType<PlayerController>();
    }
    void Start()
    {
        currentPhase = GamePhase.FirstPhase;
        currentState = GameStates.GameStart;
    }
    private void OnEnable()
    {
        OnGamePhaseStart += GameManager_OnGamePhaseStart;
        OnPlayerPassedPhase += GameManager_OnPlayerPassedPhase;
        OnGameOverPhase += GameManager_OnGameOverPhase;
        PlayerController.OnPlayerTeleportNextPhase += PlayerController_OnPlayerTeleportNextPhase;
    }
    private void OnDisable()
    {
        OnGamePhaseStart -= GameManager_OnGamePhaseStart;
        OnPlayerPassedPhase -= GameManager_OnPlayerPassedPhase;
        OnGameOverPhase -= GameManager_OnGameOverPhase;
        PlayerController.OnPlayerTeleportNextPhase -= PlayerController_OnPlayerTeleportNextPhase;
    }
  
    private void GameManager_OnGamePhaseStart()
    {
        ChangeState(GameStates.InGame);
    }
    private void GameManager_OnPlayerPassedPhase()
    {
        ChangeState(GameStates.PassedPhase);
    }
    private void GameManager_OnGameOverPhase()
    {
        ChangeState(GameStates.GameOver);
    }
    private void PlayerController_OnPlayerTeleportNextPhase()
    {
        ChangeState(GameStates.GameStart);
        SetNextPhase();
    }

    public static void StartNewPhase()
    {
        OnGamePhaseStart?.Invoke();
    }
    public static void PlayerPassedCurrentPhase()
    {
        OnPlayerPassedPhase?.Invoke();
    }
    public static void GameOverPhase()
    {
        OnGameOverPhase?.Invoke();
    }
    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case GameStates.GameStart:
                SetPlayerMovementDirection();
                //Add Interactable Object and Start the game!
                break;
            case GameStates.InGame:
                SetPlayerMovementDirection();
                //Playing and This state can change GameOver or Waiting
                break;
            case GameStates.GameOver:
                //Add lose animation for player.
                break;
            case GameStates.PassedPhase:
                //Add some particles..
                break;
            case GameStates.GameWin:
                //Add win animation for player.
                break;
        }
    }
    private void SetPlayerMovementDirection()
    {
        horizontalInput = Input.GetAxisRaw(Consts.InputConts.HORIZONTAL_INPUT);
        verticalInput = Input.GetAxisRaw(Consts.InputConts.VERTICAL_INPUT);

        switch (phaseIndex)
        {
            case 0:
                currentPhase = GamePhase.FirstPhase;
                playerController.SetPlayerDirection(new Vector3(horizontalInput, 0f, verticalInput));
                break;
            case 1:
                currentPhase = GamePhase.SecondPhase;
                playerController.SetPlayerDirection(new Vector3(verticalInput, 0f, -horizontalInput));
                break;
            case 2:
                currentPhase = GamePhase.ThirdPhase;
                playerController.SetPlayerDirection(new Vector3(-horizontalInput, 0f, -verticalInput));
                break;
            case 3:
                currentPhase = GamePhase.LastPhase;
                playerController.SetPlayerDirection(new Vector3(-verticalInput, 0f, horizontalInput));
                break;
        }
    }
    private void SetNextPhase()
    {
        phaseIndex++;
    }
    public int GetPhaseIndex()
    {
        return phaseIndex;
    }

    private void ChangeState(GameStates state)
    {
        if (currentState == state)
            return;

        currentState = state;
    }
}
