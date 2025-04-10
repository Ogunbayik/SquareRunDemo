using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private PlayerStateController stateController;
    private PlayerAnimationController animationController;
    private ColorfulGrounds colorfulGrounds;

    [Header("Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private float maxAfkTime;
    [SerializeField] private float maxStamina;

    private float afkTimer;
    private float currentStamina;

    private bool isRunning;
    private bool isWalking;
    private bool isTired;
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
        stateController = GetComponent<PlayerStateController>();
        animationController = GetComponentInChildren<PlayerAnimationController>();
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        colorfulGrounds = GameObject.FindObjectOfType<ColorfulGrounds>();
    }
    void Start()
    {
        currentStamina = maxStamina;
        afkTimer = maxAfkTime;
        skinnedMeshRenderer.material.color = GameColorManager.Instance.GetStartColor();
    }
    private void OnEnable()
    {
        GameManager.OnTeleport += Teleport;
    }
    private void OnDisable()
    {
        GameManager.OnTeleport -= Teleport;
    }
    void Update()
    {
        SetStates();
        PlayerBehaviour();
        CheckStamina();
    }
    private void SetStates()
    {
        if (IsWalking())
        {
            if (!IsRunning())
                stateController.ChangeState(PlayerStates.Walking);
            else
                stateController.ChangeState(PlayerStates.Running);
        }
        else
        {
            stateController.ChangeState(PlayerStates.Idle);
        }
    }
    private void PlayerBehaviour()
    {
        var currentState = stateController.GetCurrentState();

        switch(currentState)
        {
            case PlayerStates.Idle:
                PlayerIdle();
                break;
            case PlayerStates.Walking:
                PlayerWalking();
                break;
            case PlayerStates.Running:
                PlayerRunning();
                break;
        }
    }

    private void PlayerIdle()
    {
        CheckPlayerAfk();

        animationController.ActivateWalkAnimation(false);
        animationController.ActivateRunAnimation(false);
    }

    private void PlayerWalking()
    {
        ResetSadAnimation();

        Movement();
    }

    private void PlayerRunning()
    {
        ResetSadAnimation();
        Movement();
    }

    private void CheckStamina()
    {
        if (IsRunning())
        {
            currentStamina -= Time.deltaTime;

            if (currentStamina <= 0)
            {
                currentStamina = 0;
                isTired = true;
            }
        }
        else
        {
            currentStamina += Time.deltaTime;

            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                isTired = false;
            }
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
    private void Movement()
    {
        var movementDirection = GameManager.Instance.GetMovementDirectionNormalized();

        var movementSpeed = stateController.GetCurrentState() switch
        {
            PlayerStates.Walking => walkSpeed,
            PlayerStates.Running => runSpeed,
            _ => 0f
        };

        playerRb.velocity = movementDirection * movementSpeed;
    }
    private bool IsWalking()
    {
        var movementDirection = GameManager.Instance.GetMovementDirectionNormalized();
        animationController.ActivateWalkAnimation(isWalking);
        return isWalking = movementDirection != Vector3.zero;
    }

    private bool IsRunning()
    {
        if (!isTired)
        {
            isRunning = Input.GetKey(KeyCode.LeftShift);
            animationController.ActivateRunAnimation(isRunning);
        }
        else
        {
            isRunning = false;
            animationController.ActivateRunAnimation(isRunning);
        }

        return isRunning;
    }

    private void Teleport()
    {
        var teleportPosition = colorfulGrounds.GetNextColorfulGround();

        //Add animation delay timer
        transform.position = teleportPosition.position;
    }

    private void ResetSadAnimation()
    {
        animationController.ActivateSadAnimation(false);
        afkTimer = maxAfkTime;
    }





}
