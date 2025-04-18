using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    private SkinnedMeshRenderer skinnedMeshRenderer;
    private PlayerAnimationController animationController;
    private ColorfulGrounds colorfulGrounds;

    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private KeyCode runKey;
    [Header("Timer Settings")]
    [SerializeField] private float maxStamina;
    [SerializeField] private float teleportDelayTime;
    [SerializeField] private float afkCountDown;

    private float initTeleportDelayTime;
    private float afkTimer;
    private float currentStamina;

    private bool isAfk;
    private bool isTired;
    private bool isTeleporting;

    private Color playerColor;

    private Vector3 movementDirection;
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
        CheckPlayerStamina();

        SetPlayerStates();
        PlayerBehaviour();
    }

    private void SetPlayerStates()
    {
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
        if (isAfk)
            return;

        if (GameManager.Instance.currentState == GameManager.GameStates.InGame)
            CheckPlayerAfk();
    }

    private void PlayerWalking()
    {
        ResetSadAnimation();
        Movement(movementDirection);
    }

    private void PlayerRunning()
    {
        ResetSadAnimation();

        if (!isTired)
            Movement(movementDirection);
    }

    private void CheckPlayerStamina()
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
            isAfk = true;
        }
    }
    private void Movement(Vector3 direction)
    {
        var movementSpeed = PlayerStateManager.Instance.currentState switch
        {
            PlayerStateManager.PlayerStates.Walking => walkSpeed,
            PlayerStateManager.PlayerStates.Running => runSpeed,
            _ => 0f
        };

        playerRb.velocity = direction * movementSpeed;
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
        isAfk = false;
    }

    public void SetPlayerDirection(Vector3 direction)
    {
        movementDirection = direction;
    }

    public Color GetPlayerColor()
    {
        return playerColor;
    }



}
