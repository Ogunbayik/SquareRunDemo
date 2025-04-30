using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HandleScoreUI : MonoBehaviour
{
    private Animator animator;

    [Header("Gamebar Settings")]
    [SerializeField] private Image gameoverFill;
    [SerializeField] private Image gamepassFill;
    [Header("Score Settings")]
    [SerializeField] private TextMeshProUGUI gameScoreText;
    [SerializeField] private TextMeshProUGUI decreasedText;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    void Start()
    {
        SetScoreText(); 
        gamepassFill.fillAmount = (float)ScoreManager.gameScore / ScoreManager.Instance.GetMaxPassScore();
        gameoverFill.fillAmount = (float)Mathf.Abs(ScoreManager.gameScore) / Mathf.Abs(ScoreManager.Instance.GetMaxGameOverScore());
    }
    private void OnEnable()
    {
        ScoreManager.OnScoreDecreasedPerMinutes += ScoreManager_OnScoreDecreasedPerMinutes;
        ScoreManager.OnGameScoreIncreased += ScoreManager_OnGameScoreIncreased;
        ScoreManager.OnGameScoreDecreased += ScoreManager_OnGameScoreDecreased;
        GameManager.OnPlayerPassedPhase += GameManager_OnPlayerPassedPhase;
    }


    private void OnDisable()
    {
        ScoreManager.OnScoreDecreasedPerMinutes -= ScoreManager_OnScoreDecreasedPerMinutes;
        ScoreManager.OnGameScoreIncreased -= ScoreManager_OnGameScoreIncreased;
        ScoreManager.OnGameScoreDecreased -= ScoreManager_OnGameScoreDecreased;
        GameManager.OnPlayerPassedPhase -= GameManager_OnPlayerPassedPhase;
    }
    private void ScoreManager_OnScoreDecreasedPerMinutes()
    {
        ShowDecreaseText();
    }
    private void ScoreManager_OnGameScoreIncreased(int increaseScore)
    {
        var gameScore = ScoreManager.gameScore;
        var maxPassScore = ScoreManager.Instance.GetMaxPassScore();
        var maxGOScore = ScoreManager.Instance.GetMaxGameOverScore();

        if(gameScore >= 0)
        {
            var scoreRate = (float) gameScore / maxPassScore;
            gamepassFill.fillAmount = scoreRate;
            gameoverFill.fillAmount = 0;
            Debug.Log(scoreRate);
        }
        else
        {
            var scoreRate = (float) gameScore / maxGOScore;
            gameoverFill.fillAmount = scoreRate;
            gamepassFill.fillAmount = 0;
        }

        SetScoreText();
    }
    private void ScoreManager_OnGameScoreDecreased(int score)
    {
        var gameScore = ScoreManager.gameScore;
        var maxGOScore = ScoreManager.Instance.GetMaxGameOverScore();
        var maxPassScore = ScoreManager.Instance.GetMaxPassScore();

        if(gameScore <= 0)
        {
            var scoreRate = (float) gameScore / maxGOScore;
            gameoverFill.fillAmount = scoreRate;
            gamepassFill.fillAmount = 0;
        }
        else
        {
            var scoreRate = (float) gameScore / maxPassScore;
            gamepassFill.fillAmount = scoreRate;
            gameoverFill.fillAmount = 0;
        }

        SetScoreText();
    }
    private void SetScoreText()
    {
        gameScoreText.text = $"Score: {ScoreManager.gameScore}";
    }
    private void GameManager_OnPlayerPassedPhase()
    {
        ResetGamescore();
    }
    public void ResetGamescore()
    {
        gameoverFill.fillAmount = 0;
        gamepassFill.fillAmount = 0;
        ScoreManager.gameScore = 0;
        SetScoreText();
    }
    private void ShowDecreaseText()
    {
        decreasedText.text = $"Gamescore Decreased";
        animator.SetTrigger(Consts.GamescoreCanvasAnimationParameter.DECREASED_PARAMETER);
    }


}
