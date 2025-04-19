using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameoverCanvas : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    private void OnEnable()
    {
        ScoreManager.OnGameoverScoreChange += ScoreManager_OnGameoverScoreChange;
        GameManager.OnGameover += ScoreManager_OnGameover;
    }
    private void OnDisable()
    {
        ScoreManager.OnGameoverScoreChange -= ScoreManager_OnGameoverScoreChange;
        GameManager.OnGameover -= ScoreManager_OnGameover;
    }
    private void ScoreManager_OnGameover()
    {
        //When game is over, Show the gameover text
        animator.SetTrigger(Consts.GameoverCanvasAnimationParameter.GAMEOVER_PARAMETER);
    }
    private void ScoreManager_OnGameoverScoreChange()
    {
        ShowGameoverScore();
    }

    private void ShowGameoverScore()
    {
        animator.SetTrigger(Consts.GameoverCanvasAnimationParameter.SHOWING_GOSCORE_PARAMETER);
    }
    public void ReturnIdleGOScore()
    {
        animator.SetTrigger(Consts.GameoverCanvasAnimationParameter.RETURN_IDLE_GOSCORE_PARAMETER);
    }
}
