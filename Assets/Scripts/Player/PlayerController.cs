using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private PlayerAnimationController animationController;
    private ColorfulGrounds colorfulGrounds;

    [Header("Movement Speed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private KeyCode runKey;
    [Header("Timer Settings")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float teleportDelayTime;
    [SerializeField] private float afkCountDown;

    private PlayerStates currentState;
    private PlayerStates newState;

    private float initTeleportDelayTime;
    private float afkTimer;
    private float currentStamina;
   
    private bool isTired;
    private bool isTeleporting;

    private Color playerColor;
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody>();
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
        playerColor = skinnedMeshRenderer.material.color;

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
        CheckStamina();
        SetPlayerStates();
        PlayerBehaviour();
    }

    private void SetPlayerStates()
    {
        var movementDirection = GameManager.Instance.GetPlayerMovementDirection();

        if (movementDirection.magnitude > 0.1f)
        {
            if (!Input.GetKey(runKey))
            {
                PlayerStateManager.Instance.ChangeState(PlayerStateManager.PlayerStates.Walking);
            }
            else
            {
                if (!isTired)
                    PlayerStateManager.Instance.ChangeState(PlayerStateManager.PlayerStates.Running);
                else
                    PlayerStateManager.Instance.ChangeState(PlayerStateManager.PlayerStates.Walking);
            }
        }
        else
        {
            PlayerStateManager.Instance.ChangeState(PlayerStateManager.PlayerStates.Idle);
        }
    }

    private void PlayerBehaviour()
    {
        var currentState = PlayerStateManager.Instance.currentState;

        switch (currentState)
        {
            case PlayerStateManager.PlayerStates.Idle:
                PlayerIdle();
                break;
            case PlayerStateManager.PlayerStates.Walking:
                PlayerWalking();
                break;
            case PlayerStateManager.PlayerStates.Running:
                PlayerRunning();
                break;
        }
    }

    private void PlayerIdle()
    {
        if (GameManager.Instance.currentState == GameManager.GameStates.InGame)
            CheckPlayerAfk();
    }

    private void PlayerWalking()
    {
        ResetSadAnimation();
        Movement();
    }

    private void PlayerRunning()
    {
        ResetSadAnimation();

        if (!isTired)
            Movement();
    }

    private void CheckStamina()
    {
        if (!isTired)
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

        var movementSpeed = PlayerStateManager.Instance.currentState switch
        {
            PlayerStateManager.PlayerStates.Walking => walkSpeed,
            PlayerStateManager.PlayerStates.Running => runSpeed,
            _ => 0f
        };

        playerRb.velocity = movementDirection * movementSpeed;
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
        if (afkTimer != afkCountDown)
        {
            animationController.ActivateSadAnimation(false);
            afkTimer = afkCountDown;
        }
    }
    public Color GetPlayerColor()
    {
        return playerColor;
    }



}
