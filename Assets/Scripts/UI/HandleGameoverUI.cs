using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandleGameoverUI : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        GameManager.OnGameOverPhase += GameManager_OnGameOverPhase;
    }
    private void OnDisable()
    {
        GameManager.OnGameOverPhase -= GameManager_OnGameOverPhase;
    }
    private void GameManager_OnGameOverPhase()
    {
        //When game is over, Show the gameover text
        animator.SetTrigger(Consts.GameoverCanvasAnimationParameter.GAMEOVER_PARAMETER);
    }


}
