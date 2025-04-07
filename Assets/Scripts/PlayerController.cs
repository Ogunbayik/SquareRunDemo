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
    [SerializeField] private float maxStamina;

    private float afkTimer;
    private float currentStamina;

    private bool isRunning;
    private bool isTired;
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        stateController = GetComponent<PlayerStateController>();
        animationController = GetComponentInChildren<PlayerAnimationController>();
    }
    void Start()
    {
        afkTimer = maxAfkTime;
        currentStamina = maxStamina;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (!isTired)
                isRunning = !isRunning;
            else
                isRunning = false;
        }

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
                IdleState(movementDirection);
                break;
            case PlayerStates.Walking:
                WalkState(movementDirection);
                break;
            case PlayerStates.Running:
                RunState(movementDirection);
                break;
        }
    }

    private void IdleState(Vector3 movementDirection)
    {
        CheckPlayerAfk();
        RecoverStamina();

        if (movementDirection != Vector3.zero && !isRunning)
        {
            //Player is walking
            stateController.ChangeState(PlayerStates.Walking);
            animationController.ActivateWalkAnimation(true);
        }
        else if (movementDirection != Vector3.zero && isRunning)
        {
            //Player is running
            stateController.ChangeState(PlayerStates.Running);
            animationController.ActivateRunAnimation(true);
            animationController.ActivateWalkAnimation(true);
        }
    }

    private void CheckPlayerAfk()
    {
        afkTimer -= Time.deltaTime;
        if (afkTimer <= 0)
        {
            animationController.ActivateSadAnimation(true);
            afkTimer = 0;
        }
    }

    private void WalkState(Vector3 movementDirection)
    {
        ResetSadAnimation();
        RecoverStamina();

        if (movementDirection != Vector3.zero && isRunning)
        {
            //Player is running
            stateController.ChangeState(PlayerStates.Running);
            animationController.ActivateRunAnimation(true);
        }
        else if (movementDirection == Vector3.zero)
        {
            //Player is idle
            stateController.ChangeState(PlayerStates.Idle);
            animationController.ActivateWalkAnimation(false);
        }
    }

    private void RecoverStamina()
    {
        isTired = false;

        if (currentStamina < maxStamina)
            currentStamina += Time.deltaTime;
        else
            currentStamina = maxStamina;
    }
    private void RunState(Vector3 movementDirection)
    {
        ResetSadAnimation();
        CheckPlayerStamina();

        if (movementDirection != Vector3.zero && !isRunning)
        {
            //Player is walking
            stateController.ChangeState(PlayerStates.Walking);
            animationController.ActivateRunAnimation(false);
        }
        else if (movementDirection == Vector3.zero)
        {
            //Player is idle
            stateController.ChangeState(PlayerStates.Idle);
            animationController.ActivateRunAnimation(false);
            animationController.ActivateWalkAnimation(false);
        }
    }

    private void CheckPlayerStamina()
    {
        currentStamina -= Time.deltaTime;

        if (currentStamina <= 0)
        {
            isTired = true;
            isRunning = false;
            currentStamina += Time.deltaTime;
        }
        else if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
            isTired = false;
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
