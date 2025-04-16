using System.Collections;
using System.Collections.Generic;
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

    private int gameScore = 10;
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
        desiredGameScore = 0;

        InvokeRepeating(nameof(UpdateDesiredScore),maxCountdown, maxCountdown);
    }
    private void UpdateDesiredScore()
    {
        desiredGameScore += desiredScoreAdd;

        if (desiredGameScore <= gameScore)
            return;

        CheckGameScore();
    }
    private void OnEnable()
    {
        Gem.OnPlayerCollectGem += Gem_OnPlayerCollectGem;
        SpikeLog.OnSpikeHitPlayer += SpikeLog_OnSpikeHitPlayer;
    }

    private void OnDisable()
    {
        Gem.OnPlayerCollectGem -= Gem_OnPlayerCollectGem;
        SpikeLog.OnSpikeHitPlayer -= SpikeLog_OnSpikeHitPlayer;
    }

    private void Gem_OnPlayerCollectGem(Gem gem, PlayerController player)
    {
        gameScore += gem.GetGemScore();

        CheckGameScore();
    }
    private void SpikeLog_OnSpikeHitPlayer(SpikeLog spike, PlayerController player)
    {
        gameScore -= spike.GetDecreaseScore();

        CheckGameScore();
    }

    private void CheckGameScore()
    {
        if(gameScore >= passScore)
        {
            //Player passed currentPhase and Teleporting
            //Change GameManagerState
            OnPassedPhase?.Invoke();

            var multiplyPassScore = 3;
            passScore *= multiplyPassScore;
        }
        else if(gameScore < desiredGameScore)
        {
            Debug.Log("Game is over");
            CancelInvoke(nameof(UpdateDesiredScore));
        }
    }

    
}
