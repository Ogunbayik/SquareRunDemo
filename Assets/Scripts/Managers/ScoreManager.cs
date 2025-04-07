using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

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
