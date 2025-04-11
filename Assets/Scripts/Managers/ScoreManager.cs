using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public static Action OnPassedPhase;

    [Header("Settings")]
    [SerializeField] private float maxCountdown;
    [SerializeField] private int desiredScoreAdd;
    [SerializeField] private int passScore;

    private float countdownTimer;

    private int gameScore;
    private int desiredGameScore;
    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
            Instance = this;
        #endregion

        countdownTimer = maxCountdown;
    }
    private void Update()
    {
        IncreaseDesiredScore();
        CheckGameScore();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameScore += 10;
            Debug.Log(gameScore);
        }
    }

    private void CheckGameScore()
    {
        if (gameScore < desiredGameScore)
        {
            //Game is over.
            //Add game over animation.
        }
        else if (gameScore >= passScore)
        {
            OnPassedPhase?.Invoke();

            var multiplyNextPassScore = 2;
            passScore *= multiplyNextPassScore;
            //Player passed the current phase.
            //Player teleporting next coloful ground.
        }
    }

    private void IncreaseDesiredScore()
    {
        countdownTimer -= Time.deltaTime;

        if(countdownTimer <= 0)
        {
            desiredGameScore += desiredScoreAdd;
            countdownTimer = maxCountdown;
        }
    }
    private void OnEnable()
    {
        SpikeLog.OnHitPlayer += UpdateGameScore;
        Gem.OnGemCollected += UpdateGameScore;
    }

    private void OnDisable()
    {
        SpikeLog.OnHitPlayer -= UpdateGameScore;
        Gem.OnGemCollected -= UpdateGameScore;
    }

    private void UpdateGameScore(int score)
    {
        gameScore += score;
        Debug.Log(gameScore);
    }

    
}
