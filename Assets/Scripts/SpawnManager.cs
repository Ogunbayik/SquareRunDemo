using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private ColorfulGround currentColorfulGround;

    [SerializeField] private GameObject logPrefab;
    [SerializeField] private float maxSpawnTimer;

    private float spawnTimer;

    private Vector3 randomPosition;
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
        spawnTimer = maxSpawnTimer;
    }

    void Update()
    {
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
}
