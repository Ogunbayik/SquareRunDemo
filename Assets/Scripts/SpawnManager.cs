using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private ColorfulGround currentColorfulGround;

    [Header("Settings")]
    [SerializeField] private GameObject logPrefab;
    [SerializeField] private float maxSpawnTimer;

    private float spawnTimer;
    private bool canSpawn;
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
        if (canSpawn)
            SpawnLogs();
    }

    private void SpawnLogs()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0)
        {
            CreateLog();
            spawnTimer = maxSpawnTimer;
        }
    }

    private void CreateLog()
    {
        var log = Instantiate(logPrefab);
        log.transform.position = currentColorfulGround.GetRandomPosition();
        log.transform.rotation = currentColorfulGround.GetRotation();
    }

    public void SetCurrentColorfulGround(ColorfulGround colorfulGround)
    {
        if (currentColorfulGround == colorfulGround)
            return;

        currentColorfulGround = colorfulGround;
    }

    public void ActivateSpawn(bool isActive)
    {
        canSpawn = isActive;
    }

    public void ResetSpawning()
    {
        spawnTimer = maxSpawnTimer;
        canSpawn = false;
    }
}
