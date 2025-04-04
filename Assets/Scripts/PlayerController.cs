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

    private float afkTimer;

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
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = !isRunning;

        Movement();
        SetStates();
    }

    private void SetStates()
    {
        var currentState = stateController.GetCurrentState();
        var movementDirection = GameManager.Instance.GetMovementDirectionNormalized();

        switch (currentState)
        {
            case PlayerStates.Idle:
                afkTimer -= Time.deltaTime;
                if (afkTimer <= 0)
                {
                    animationController.ActivateSadAnimation(true);
                    afkTimer = 0;
                }

                if (movementDirection != Vector3.zero && !isRunning)
                {
                    stateController.ChangeState(PlayerStates.Walking);
                    animationController.ActivateWalkAnimation(true);
                }
                else if (movementDirection != Vector3.zero && isRunning)
                {
                    stateController.ChangeState(PlayerStates.Running);
                    animationController.ActivateRunAnimation(true);
                    animationController.ActivateWalkAnimation(true);
                }
                break;
            case PlayerStates.Walking:
                ResetSadAnimation();

                if (movementDirection != Vector3.zero && isRunning)
                {
                    stateController.ChangeState(PlayerStates.Running);
                    animationController.ActivateRunAnimation(true);
                }
                else if (movementDirection == Vector3.zero)
                {
                    stateController.ChangeState(PlayerStates.Idle);
                    animationController.ActivateWalkAnimation(false);
                }
                break;
            case PlayerStates.Running:
                ResetSadAnimation();

                if (movementDirection != Vector3.zero && !isRunning)
                {
                    stateController.ChangeState(PlayerStates.Walking);
                    animationController.ActivateRunAnimation(false);
                }
                else if (movementDirection == Vector3.zero)
                {
                    stateController.ChangeState(PlayerStates.Idle);
                    animationController.ActivateRunAnimation(false);
                    animationController.ActivateWalkAnimation(false);
                }
                break;
        }
    }

    private void Movement()
    {
        var movementDirection = GameManager.Instance.GetMovementDirectionNormalized();

        var movementSpeed = stateController.GetCurrentState() switch
        {
            PlayerStates.Walking => walkSpeed,
            PlayerStates.Running => runSpeed,
            _ => 1f
        };

        if (movementDirection != Vector3.zero)
            playerRb.velocity = movementDirection * movementSpeed;


    }
    private void ResetSadAnimation()
    {
        animationController.ActivateSadAnimation(false);
        afkTimer = maxAfkTime;
    }
}
