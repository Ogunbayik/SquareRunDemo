using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private ColorfulGround currentColorfulGround;
    public enum GamePhase
    {
        FirstPhase,
        SecondPhase,
        ThirdPhase,
        LastPhase
    }

    public GamePhase currentPhase;

    private Vector3 playerMovementDirection;

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

        currentColorfulGround = null;
    }
    void Start()
    {
        currentPhase = GamePhase.FirstPhase;
    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxisRaw(Consts.InputConts.HORIZONTAL_INPUT);
        verticalInput = Input.GetAxisRaw(Consts.InputConts.VERTICAL_INPUT);

        switch(currentPhase)
        {
            case GamePhase.FirstPhase:
                playerMovementDirection = new Vector3(horizontalInput, 0f, verticalInput);
                break;
            case GamePhase.SecondPhase:
                playerMovementDirection = new Vector3(verticalInput, 0f, -horizontalInput);
                break;
            case GamePhase.ThirdPhase:
                playerMovementDirection = new Vector3(-horizontalInput, 0f, -verticalInput);
                break;
            case GamePhase.LastPhase:
                playerMovementDirection = new Vector3(-verticalInput, 0f, horizontalInput);
                break;
        }
    }

    public void SetCurrentColorfulGround(ColorfulGround colorfulGround)
    {
        if (currentColorfulGround == colorfulGround)
            return;

        currentColorfulGround = colorfulGround;
    }
    public void ChangePhase(GamePhase phase)
    {
        if(currentPhase == phase) { return; }

        currentPhase = phase;
    }


    public Vector3 GetMovementDirectionNormalized()
    {
        return playerMovementDirection.normalized;
    }
}
