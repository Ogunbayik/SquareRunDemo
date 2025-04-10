using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [SerializeField] private int passScore;
    private int currentScore = 10;
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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UpgradeScore(10);
        }

        if (currentScore >= passScore && GameManager.Instance.currentState == GameManager.GameStates.InGame)
        {
            GameManager.Instance.ResetDelayTimer();
            GameManager.Instance.ChangeState(GameManager.GameStates.Teleporting);
        }
    }

    private void OnEnable()
    {
        SpikeLog.OnHitPlayer += UpgradeScore;
        Gem.OnGemCollected += UpgradeScore;
    }

    private void OnDisable()
    {
        SpikeLog.OnHitPlayer -= UpgradeScore;
        Gem.OnGemCollected -= UpgradeScore;
    }

    private void UpgradeScore(int score)
    {
        currentScore += score;
        Debug.Log(currentScore);
    }

    
}
