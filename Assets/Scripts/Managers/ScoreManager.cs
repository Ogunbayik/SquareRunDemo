using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public static Action OnPassedPhase;

    [Header("Settings")]
    [SerializeField] private float firstCountdown;
    [SerializeField] private float repeatCountdown;
    [SerializeField] private int addScore;
    [SerializeField] private int passScore;

    private int gameScore;
    private int gameOverScore;
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

        gameOverScore = 0;
    }
    private void Start()
    {
        InvokeRepeating(nameof(UpdateGameOverScore), firstCountdown, repeatCountdown);
    }
    
    private void UpdateGameOverScore()
    {
        gameOverScore += addScore;

        if (gameOverScore <= gameScore)
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

    private void Gem_OnPlayerCollectGem(Gem gem, PlayerInteraction player)
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
        else if(gameScore < gameOverScore)
        {
            Debug.Log("Game is over");
            CancelInvoke(nameof(UpdateGameOverScore));
        }
    }

    
}
