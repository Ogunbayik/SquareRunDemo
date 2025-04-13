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

    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [Header("Timer Settings")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float teleportDelayTime;
    [SerializeField] private float afkCountDown;

    private float initTeleportDelayTime;
    private float afkTimer;
    private float currentStamina;
   
    private bool isRunning;
    private bool isWalking;
    private bool isTired;
    private bool isTeleporting;
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
        initTeleportDelayTime = teleportDelayTime;
        afkTimer = afkCountDown;
        skinnedMeshRenderer.material.color = GameColorManager.Instance.GetStartColor();
    }
    private void OnEnable()
    {
        ScoreManager.OnPassedPhase += PlayerIsTeleporting;
    }
    private void OnDisable()
    {
        ScoreManager.OnPassedPhase -= PlayerIsTeleporting;
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
        if (GameManager.Instance.currentState == GameManager.GameStates.InGame)
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
        var movementDirection = GameManager.Instance.GetPlayerMovementDirection();

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
        var movementDirection = GameManager.Instance.GetPlayerMovementDirection();
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

    private void PlayerIsTeleporting()
    {
        isTeleporting = true;
        StartCoroutine(nameof(TeleportNextArea));
    }

    private IEnumerator TeleportNextArea()
    {
        while (isTeleporting)
        {
            var teleportPosition = colorfulGrounds.GetNextColorfulGround();
            yield return new WaitForSeconds(teleportDelayTime);
            isTeleporting = false;
            transform.position = teleportPosition.position;
            teleportDelayTime = initTeleportDelayTime;
        }
    }
    private void ResetSadAnimation()
    {
        animationController.ActivateSadAnimation(false);
        afkTimer = afkCountDown;
    }





}
