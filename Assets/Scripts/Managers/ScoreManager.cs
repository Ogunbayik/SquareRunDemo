using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public static event Action OnGameoverScoreChange;

    [Header("Settings")]
    [SerializeField] private float countDownFirst;
    [SerializeField] private float countDownRepeat;
    [SerializeField] private int addScore;
    [SerializeField] private int passScore;
    [SerializeField] private int multiplyBoost;
    [SerializeField] private int multiplyDecreased;
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI gameScoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    private int gameScore = 8;
    private int gameOverScore = 0;
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
        InvokeRepeating(nameof(UpdateGameOverScore), countDownFirst, countDownRepeat);
        gameScoreText.text = $"Score: {gameScore}";
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
        var playerMode = player.GetComponent<PlayerGroundCheck>().currentMode;

        if(playerMode == PlayerGroundCheck.PlayerMode.Boosted)
            gameScore += gem.GetGemScore() * multiplyBoost;
        else if(playerMode == PlayerGroundCheck.PlayerMode.Decreased)
            gameScore += gem.GetGemScore() * multiplyDecreased;

        UpdateGameScoreText(gameScore);
        CheckGameScore();
    }
    private void SpikeLog_OnSpikeHitPlayer(SpikeLog spike, PlayerController player)
    {
        var playerColor = player.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
        var spikeColor = spike.GetSpikeColor();
        var decreaseMultiply = 2;

        if (playerColor.r == spikeColor.r && playerColor.g == spikeColor.g && playerColor.b == spikeColor.b)
            gameScore -= spike.GetDecreaseScore();
        else
            gameScore -= spike.GetDecreaseScore() * decreaseMultiply;

        UpdateGameScoreText(gameScore);
        CheckGameScore();
    }
    private void UpdateGameScoreText(int score)
    {
        gameScoreText.text = $"Score: {score}";
    }
    private void UpdateGameOverScore()
    {
        OnGameoverScoreChange?.Invoke();

        gameOverScore += addScore;
        UpdateGameOverScoreText(gameOverScore);

        if (gameOverScore <= gameScore)
            return;

        CheckGameScore();
    }
    private void UpdateGameOverScoreText(int score)
    {
        gameOverScoreText.text = $"Gameover Score Updated: {score}";
    }
    private void CheckGameScore()
    {
        if(gameScore >= passScore)
        {
            GameManager.PlayerPassedCurrentPhase();
            //Player passed currentPhase and Teleporting
            //Change GameManagerState
            

            var multiplyPassScore = 3;
            passScore *= multiplyPassScore;
        }
        else if(gameScore < gameOverScore)
        {
            GameManager.GameOverPhase();
            //We dont need to countdown after game over.
            CancelInvoke(nameof(UpdateGameOverScore));
            Debug.Log("Game is over");
        }
    }

    
}
