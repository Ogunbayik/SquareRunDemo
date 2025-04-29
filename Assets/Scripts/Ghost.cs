using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GhostStates
{
    Idle,
    Move
}
public class Ghost : MonoBehaviour , IHitable
{
    public static event Action<GhostStates, GhostStates> OnGhostStateChanged;

    private Collider ghostCollider;

    private GhostStates currentState;

    [Header("Movement Settings")]
    [SerializeField] private bool isMoveVertical;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float minimumRange;
    [SerializeField] private float maximumRange;
    [SerializeField] private float maxWaitingTime;
    [Header("Scale Settings")]
    [SerializeField] private float increaseSpeed;
    [SerializeField] private Vector3 increaseSize;
    [SerializeField] private Vector3 maxGhostSize;
    [Header("Score Settings")]
    [SerializeField] private int decreaseScore;

    private Vector3 startPosition;
    private Vector3 minimumPosition;
    private Vector3 maximumPosition;
    private Vector3 movementPosition;
    private Vector3 desiredGhostScale;

    private float waitingTimer;

    private bool isMove = true;
    private bool isIncreasing = false;
    private void Awake()
    {
        ghostCollider = GetComponent<Collider>();
    }
    void Start()
    {
        startPosition = transform.position;
        waitingTimer = maxWaitingTime;
        SetMovementPosition();
    }

    void Update()
    {
        SetGhostBehaviour();

        if (isIncreasing)
            IncreasingScale();
    }
    private void SetGhostBehaviour()
    {
        if (isMove)
            SetGhostState(GhostStates.Move);
        else
            SetGhostState(GhostStates.Idle);

        switch(currentState)
        {
            case GhostStates.Idle:
                SetNextPosition();
                break;
            case GhostStates.Move:
                transform.position = Vector3.MoveTowards(transform.position, movementPosition, movementSpeed * Time.deltaTime);

                CheckMovementDistance();
                break;
        }
    }
    private void SetNextPosition()
    {
        waitingTimer -= Time.deltaTime;

        if(waitingTimer <= 0)
        {
            if (movementPosition == minimumPosition)
                movementPosition = maximumPosition;
            else
                movementPosition = minimumPosition;

            waitingTimer = maxWaitingTime;
            isMove = true;
        }
    }
    private void CheckMovementDistance()
    {
        var movePosDistance = Vector3.Distance(transform.position, movementPosition);
        if(movePosDistance <= 0)
        {
            isMove = false;
            SetNextPosition();
        }
    }
    private void SetMovementPosition()
    {
        var yPosition = 0.3f;
        if(isMoveVertical)
        {
            //Ghost moving Z coordinate
            minimumPosition = new Vector3(startPosition.x, yPosition, startPosition.z - minimumRange);
            maximumPosition = new Vector3(startPosition.x, yPosition, startPosition.z + maximumRange);
        }
        else
        {
            //Ghost moving X coordinate
            minimumPosition = new Vector3(startPosition.x - minimumRange, yPosition, startPosition.z);
            maximumPosition = new Vector3(startPosition.x + maximumRange, yPosition, startPosition.z);
        }

        movementPosition = minimumPosition;
    }

    private void SetGhostState(GhostStates newState)
    {
        var previousState = currentState;

        if (previousState == newState)
            return;

        currentState = newState;
        OnGhostStateChanged?.Invoke(currentState, newState);
    }

    public GhostStates GetCurrentState()
    {
        return currentState;
    }
    public void HitPlayer(PlayerInteraction player)
    {
        ScoreManager.Instance.DecreaseGameScore(decreaseScore);

        StartCoroutine(nameof(IncreaseGhostSize));
    }
    private IEnumerator IncreaseGhostSize()
    {
        var delayIncreasingTime = 2f;
        ghostCollider.enabled = false;
        desiredGhostScale = transform.localScale + increaseSize;
        yield return new WaitForSeconds(delayIncreasingTime);
        isIncreasing = true;
        var delayActivateColliderTime = 3f;
        yield return new WaitForSeconds(delayActivateColliderTime);
        ghostCollider.enabled = true;
        CancelInvoke(nameof(IncreaseGhostSize));
    }
    private void IncreasingScale()
    {
        if (transform.localScale.x >= maxGhostSize.x)
        {
            transform.localScale = maxGhostSize;
            isIncreasing = false;
            return;
        }

        if (transform.localScale.x < desiredGhostScale.x)
        {
            transform.localScale += increaseSize * increaseSpeed * Time.deltaTime;
        }
        else
        {
            transform.localScale = new Vector3(desiredGhostScale.x, desiredGhostScale.y, desiredGhostScale.z);
            isIncreasing = false;
        }
    }

    public int GetDecreaseScore()
    {
        return decreaseScore;
    }
}
