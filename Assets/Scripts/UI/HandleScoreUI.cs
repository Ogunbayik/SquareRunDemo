using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleScoreUI : MonoBehaviour
{
    [SerializeField] private Image gameoverFill;
    [SerializeField] private Image gamepassFill;
    void Start()
    {
        gameoverFill.fillAmount = (float)Mathf.Abs(ScoreManager.gameScore) / ScoreManager.Instance.GetMaxPassScore();
        gamepassFill.fillAmount = 0;
    }
    private void OnEnable()
    {
        //ScoreManager.OnGameScoreIncreased += ScoreManager_OnGameScoreIncreased;
        //ScoreManager.OnGameScoreDecreased += ScoreManager_OnGameScoreDecreased;
    }
    private void OnDisable()
    {
        //ScoreManager.OnGameScoreIncreased -= ScoreManager_OnGameScoreIncreased;
        //ScoreManager.OnGameScoreDecreased -= ScoreManager_OnGameScoreDecreased;
    }
    //private void ScoreManager_OnGameScoreIncreased(int increaseScore)
    //{
    //    var gameScore = ScoreManager.gameScore;
    //    var maxPassScore = ScoreManager.Instance.GetMaxPassScore();
    //    var scoreRate = (float)increaseScore / maxPassScore;
    //    Debug.Log(scoreRate);

    //    if(gameScore >= 0)
    //    {
    //        gamepassFill.fillAmount += (float)scoreRate;
    //    }
    //    else if(gameScore < 0 && gameScore >= -increaseScore)
    //    {
    //        var differenceBetweenScores = gameScore - increaseScore;
    //        var betweenScoreRate = (float) Mathf.Abs(differenceBetweenScores / maxPassScore);
    //        gamepassFill.fillAmount += (float)betweenScoreRate;
    //        gameoverFill.fillAmount = 0;
    //    }
    //    else if(gameScore < -increaseScore)
    //    {
    //        gameoverFill.fillAmount -= (float)(scoreRate);
    //    }
        
    //}
    //private void ScoreManager_OnGameScoreDecreased(int score)
    //{

    //}


}
