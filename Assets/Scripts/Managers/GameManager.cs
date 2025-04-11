using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static Action OnTeleporting;
    public static Action OnTeleported;
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

    [SerializeField] private float delayTeleporting;
    [SerializeField] private float delayNextState;

    [SerializeField] private int spikeRotationY;

    private int phaseIndex;

    private Vector3 playerMovementDirection;
    private Vector3 spikeRotation;

    private float horizontalInput;
    private float verticalInput;
    private float initialDelayTime;

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

        initialDelayTime = delayNextState;
        spikeRotation = new Vector3(0f, spikeRotationY, 0f);
    }
    private void OnEnable()
    {
        ScoreManager.OnPassedPhase += PlayerPassedPhase;
        OnTeleported += PlayerTeleported;
    }

    private void OnDisable()
    {
        ScoreManager.OnPassedPhase -= PlayerPassedPhase;
        OnTeleported -= PlayerTeleported;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerIsStopped();
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
                playerMovementDirection = new Vector3(horizontalInput, 0f, verticalInput);

                spikeRotation = new Vector3(0f, 180f, 0f);
                SpawnManager.Instance.SetSpikeRotation(spikeRotation);
                break;
            case 1:
                currentPhase = GamePhase.SecondPhase;
                playerMovementDirection = new Vector3(verticalInput, 0f, -horizontalInput);

                spikeRotation = new Vector3(0f, 270f, 0f);
                SpawnManager.Instance.SetSpikeRotation(spikeRotation);
                break;
            case 2:
                currentPhase = GamePhase.ThirdPhase;
                playerMovementDirection = new Vector3(-horizontalInput, 0f, -verticalInput);

                spikeRotation = new Vector3(0f, 0f, 0f);
                SpawnManager.Instance.SetSpikeRotation(spikeRotation);
                break;
            case 3:
                currentPhase = GamePhase.LastPhase;
                playerMovementDirection = new Vector3(-verticalInput, 0f, horizontalInput);

                spikeRotation = new Vector3(0f, 90f, 0f);
                SpawnManager.Instance.SetSpikeRotation(spikeRotation);
                break;
        }
    }

    //private void SetSpikeRotation()
    //{
    //    var spikeAddRotation = 90f;
    //    spikeRotation = new Vector3(0f, spikeRotation.y + spikeAddRotation, 0f);

    //    if (spikeRotationY == 360)
    //        spikeRotationY = 0;
    //}
    private void PlayerIsStopped()
    {
        if (currentState == GameStates.GameStart || currentState == GameStates.InGame)
            playerMovementDirection = playerMovementDirection.normalized;
        else
            playerMovementDirection = Vector3.zero;
    }
    public Vector3 GetPlayerMovementDirection()
    {
        return playerMovementDirection;
    }

    private void PlayerPassedPhase()
    {
        StartCoroutine(nameof(NextPhaseDelay));
    }
    private IEnumerator NextPhaseDelay()
    {
        ChangeState(GameStates.PassedPhase);

        yield return new WaitForSeconds(delayTeleporting);

        OnTeleporting?.Invoke();

        yield return new WaitForSeconds(delayNextState);

        OnTeleported?.Invoke();
    }

    private void PlayerTeleported()
    {
        phaseIndex++;
        ChangeState(GameStates.GameStart);
        StopCoroutine(nameof(NextPhaseDelay));
    }
    public int GetPhaseIndex()
    {
        return phaseIndex;
    }

    public void ChangeState(GameStates state)
    {
        if (currentState == state)
            return;

        currentState = state;
    }
}
