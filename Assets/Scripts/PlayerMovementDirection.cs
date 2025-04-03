using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementDirection : MonoBehaviour
{
    private PlayerController playerController;

    public enum PlayerDirection
    {
        positiveDirectionZ,
        positiveDirectionX,
        negativeDirectionZ,
        negativeDirectionX
    }

    public PlayerDirection playerDirection;

    private float horizontalInput;
    private float verticalInput;

    private Vector3 movementDirection;
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }
    void Start()
    {
        playerDirection = PlayerDirection.positiveDirectionZ;
    }

    void Update()
    {
        horizontalInput = Input.GetAxisRaw(Consts.InputConts.HORIZONTAL_INPUT);
        verticalInput = Input.GetAxisRaw(Consts.InputConts.VERTICAL_INPUT);

        switch(playerDirection)
        {
            case PlayerDirection.positiveDirectionZ:
                movementDirection = new Vector3(horizontalInput, 0f, verticalInput);
                break;
            case PlayerDirection.positiveDirectionX:
                movementDirection = new Vector3(verticalInput, 0f, -horizontalInput);
                break;
            case PlayerDirection.negativeDirectionZ:
                movementDirection = new Vector3(-horizontalInput, 0f, -verticalInput);
                break;
            case PlayerDirection.negativeDirectionX:
                movementDirection = new Vector3(-verticalInput, 0f, horizontalInput);
                break;
        }
    }

    public void ChangeMovementDirection(PlayerDirection direction)
    {
        playerDirection = direction;
    }

    public Vector3 GetMovementDirectionNormalized()
    {
        return movementDirection.normalized;
    }
}
