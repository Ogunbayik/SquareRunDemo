using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public static event Action<int> OnGameScoreDecreased;
    public static event Action<int> OnGameScoreIncreased;

    [Header("Timer Settings")]
    [SerializeField] private float countDownFirst;
    [SerializeField] private float countDownRepeat;
    [Header("Score Settings")]
    [SerializeField] private int decreaseScore;
    [SerializeField] private int maxPassScore;
    [SerializeField] private int maxGameoverScore;
    [Header("Multiply Settings")]
    [SerializeField] private int multiplyBoost;
    [SerializeField] private int multiplyDecreased;

    public static int gameScore = 0;
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
    }
    private void Start()
    {
        //GAME OVER ÝÇÝN SAYAÇ EKLE.
        InvokeRepeating(nameof(DecreaseGameScorePerMinutes), countDownFirst, countDownRepeat);
    }
    private void OnEnable()
    {
        PlayerInteraction.OnPlayerHitted += PlayerInteraction_OnHitTriggerable;
        Gem.OnPlayerCollectGem += Gem_OnPlayerCollectGem;
    }

    private void OnDisable()
    {
        Gem.OnPlayerCollectGem -= Gem_OnPlayerCollectGem;
        PlayerInteraction.OnPlayerHitted -= PlayerInteraction_OnHitTriggerable;
    }
    private void PlayerInteraction_OnHitTriggerable(PlayerInteraction player, IHitable hitable)
    {
        hitable.HitPlayer(player);

        CheckGameScore();
    }
    private void Gem_OnPlayerCollectGem(Gem gem, PlayerInteraction player)
    {
        var playerMode = player.GetComponent<PlayerGroundCheck>().currentMode;

        if (playerMode == PlayerGroundCheck.PlayerMode.Boosted)
            IncreaseGameScore(gem.GetGemScore() * multiplyBoost);
        else if(playerMode == PlayerGroundCheck.PlayerMode.Decreased)
            IncreaseGameScore(gem.GetGemScore() * multiplyDecreased);

        CheckGameScore();
    }
    private void IncreaseGameScore(int score)
    {
        gameScore += score;
        OnGameScoreIncreased?.Invoke(score);
    }
    public void DecreaseGameScore(int score)
    {
        gameScore -= score;
        OnGameScoreDecreased?.Invoke(score);
    }
    private void DecreaseGameScorePerMinutes()
    {
        DecreaseGameScore(decreaseScore);

        if (maxGameoverScore <= gameScore)
            return;

        CheckGameScore();
    }
    private void CheckGameScore()
    {
        if(gameScore >= maxPassScore)
        {
            //Player teleporting and Change Game State
            GameManager.PlayerPassedCurrentPhase();

            //Increase game passScore
            var multiplyPassScore = 3;
            maxPassScore *= multiplyPassScore;
        }
        else if (gameScore <= maxGameoverScore)
        {
            GameManager.GameOverPhase();

            //We dont need to countdown after game over.
            CancelInvoke(nameof(DecreaseGameScorePerMinutes));
            Debug.Log("Game is over");
        }
    }

    public int GetMaxGameOverScore()
    {
        return maxGameoverScore;
    }
    public int GetMaxPassScore()
    {
        return maxPassScore;
    }


    
}
