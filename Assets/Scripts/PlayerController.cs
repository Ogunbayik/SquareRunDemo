using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private PlayerStateController stateController;
    private PlayerAnimationController animationController;

    [Header("Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float maxAfkTime;

    private float horizontalInput;
    private float verticalInput;
    private float afkTimer;

    private Vector3 movementDirection;

    private bool isRunning;
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        stateController = GetComponent<PlayerStateController>();
        animationController = GetComponentInChildren<PlayerAnimationController>();
    }
    void Start()
    {
        afkTimer = maxAfkTime;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            isRunning = !isRunning;


        Movement();


        var currentState = stateController.GetCurrentState();
        var movementDirection = GetMovementDirectionNormalized();

        var newState = currentState switch
        {
            _ when movementDirection == Vector3.zero => PlayerStates.Idle,
            _ when movementDirection != Vector3.zero && !isRunning => PlayerStates.Walking,
            _ when movementDirection != Vector3.zero && isRunning => PlayerStates.Running,
            _ => currentState
        };

        if (currentState != newState)
            stateController.ChangeState(newState);
    }

    private void Movement()
    {
        horizontalInput = Input.GetAxisRaw(Consts.InputConts.HORIZONTAL_INPUT);
        verticalInput = Input.GetAxisRaw(Consts.InputConts.VERTICAL_INPUT);

        movementDirection = new Vector3(horizontalInput, 0f, verticalInput);

        if (movementDirection != Vector3.zero && !isRunning)
        {
            playerRb.velocity = movementDirection.normalized * walkSpeed;
        }
        else if (movementDirection != Vector3.zero && isRunning)
        {
            playerRb.velocity = movementDirection.normalized * runSpeed;
        }

    }

    private Vector3 GetMovementDirectionNormalized()
    {
        return movementDirection.normalized;
    }
}
