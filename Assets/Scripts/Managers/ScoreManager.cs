using System;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public static Action OnPassedPhase;
    public static event Action OnGameoverScoreChange;

    [Header("Settings")]
    [SerializeField] private float countDownFirst;
    [SerializeField] private float countDownRepeat;
    [SerializeField] private int addScore;
    [SerializeField] private int passScore;
    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI gameScoreText;
    [SerializeField] private TextMeshProUGUI gameOverScoreText;

    private int gameScore = 10;
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
        gameScore += gem.GetGemScore();
        UpdateGameScoreText(gameScore);

        CheckGameScore();
    }
    private void SpikeLog_OnSpikeHitPlayer(SpikeLog spike, PlayerController player)
    {
        gameScore -= spike.GetDecreaseScore();
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
            OnPassedPhase?.Invoke();
            

            var multiplyPassScore = 3;
            passScore *= multiplyPassScore;
        }
        else if(gameScore < gameOverScore)
        {
            GameManager.Instance.ChangeState(GameManager.GameStates.GameOver);
            //We dont need to countdown after game over.
            CancelInvoke(nameof(UpdateGameOverScore));
        }
    }

    
}
