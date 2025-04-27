using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandleGameoverUI : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private TextMeshProUGUI updateGOScoreText;

    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        
    }
    private void OnEnable()
    {
        ScoreManager.OnGameScoreDecreased += ScoreManager_OnGameScoreDecreased;
        GameManager.OnGameOverPhase += GameManager_OnGameOverPhase;
    }
    private void OnDisable()
    {
        ScoreManager.OnGameScoreDecreased -= ScoreManager_OnGameScoreDecreased;
        GameManager.OnGameOverPhase -= GameManager_OnGameOverPhase;
    }
    private void GameManager_OnGameOverPhase()
    {
        //When game is over, Show the gameover text
        animator.SetTrigger(Consts.GameoverCanvasAnimationParameter.GAMEOVER_PARAMETER);
    }
    private void ScoreManager_OnGameScoreDecreased(int score)
    {
        ShowGameoverScore(score);
    }
    private void ShowGameoverScore(int score)
    {
        animator.SetTrigger(Consts.GameoverCanvasAnimationParameter.SHOWING_GOSCORE_PARAMETER);
        updateGOScoreText.text = score.ToString();
    }
    public void ReturnIdleGOScore()
    {
        animator.SetTrigger(Consts.GameoverCanvasAnimationParameter.RETURN_IDLE_GOSCORE_PARAMETER);
    }


}
