using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [Header("Settings")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private float maxSpawnTimer;

    private float spawnTimer;
    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        #endregion
    }
    void Start()
    {
        ResetSpawning();
    }

    void Update()
    {
        if (GameManager.Instance.currentState == GameManager.GameStates.InGame)
            SpawnSpikes();
        else
            ResetSpawning();
    }

    private void SpawnSpikes()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            var log = Instantiate(spikePrefab);
            spawnTimer = maxSpawnTimer;
        }
    }

    public void ResetSpawning()
    {
        spawnTimer = maxSpawnTimer;
    }
}
