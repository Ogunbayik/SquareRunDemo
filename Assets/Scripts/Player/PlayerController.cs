using System.Collections;
using UnityEngine;
using System;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerTeleportNextPhase;

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
    [SerializeField] private float afkCountDown;
    [Header("Teleport Settings")]
    [SerializeField] private ParticleSystem teleportParticle;
    [SerializeField] private float teleportTime;
    [SerializeField] private float delayAnimationTime;

    [SerializeField] private Transform bodyTransform;
    [SerializeField] private float rotationSpeed;

    private float afkTimer;
    private float currentStamina;

    private bool isAfk;
    private bool isTired;
    private bool isTeleporting;

    private Color playerColor;

    private Vector3 movementDirection;
    private Vector3 teleportPosition;
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
        afkTimer = afkCountDown;
        skinnedMeshRenderer.material.color = GameColorManager.Instance.GetStartColor();
        playerColor = skinnedMeshRenderer.material.color;


    }
    private void OnEnable()
    {
        GameManager.OnPlayerPassedPhase += GameManager_OnPlayerPassedPhase;
    }
    private void OnDisable()
    {
        GameManager.OnPlayerPassedPhase -= GameManager_OnPlayerPassedPhase;
    }

    private void GameManager_OnPlayerPassedPhase()
    {
        StartCoroutine(nameof(PlayerTeleportNextArea));
    }

    private IEnumerator PlayerTeleportNextArea()
    {
        //Player must stop the place.
        ReturnIdle();
        gameObject.GetComponent<PlayerController>().enabled = false;
        teleportPosition = colorfulGrounds.GetNextColorfulGround().transform.position;

        yield return new WaitForSeconds(1f);
        //Use teleport animation
        animationController.ActivateTeleportAnimation();
        yield return new WaitForSeconds(3f);
        //Add particle teleport efect 
        var teleportSphere = Instantiate(teleportParticle, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(4f);
        //Change game manager state
        Destroy(teleportSphere.gameObject);
        transform.position = teleportPosition;
        yield return new WaitForSeconds(1f);
        SetPlayerDirection(movementDirection);
        isTeleporting = false;
        gameObject.GetComponent<PlayerController>().enabled = true;
        StopCoroutine(nameof(PlayerTeleportNextArea));
        OnPlayerTeleportNextPhase?.Invoke();
    }
    private void ReturnIdle()
    {
        PlayerStateManager.Instance.ChangeState(PlayerStateManager.PlayerStates.Idle);
    }

   
    void Update()
    {
        CheckPlayerStamina();

        SetPlayerStates();
        PlayerBehaviour();
        SetRotation();
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

        if (GameManager.Instance.currentState == GameManager.GameStates.InGame || GameManager.Instance.currentState == GameManager.GameStates.GameStart && !isTeleporting)
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

    private void SetRotation()
    {
        if(movementDirection != Vector3.zero)
        {
            var bodyForward = Vector3.RotateTowards(bodyTransform.forward, movementDirection, rotationSpeed * Time.deltaTime, 0f);
            var bodyRotation = Quaternion.LookRotation(bodyForward);

            bodyTransform.rotation = bodyRotation;
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
