using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject logPrefab;
    [SerializeField] private float minimumBorder;
    [SerializeField] private float maximumBorder;
    [SerializeField] private float maxSpawnTimer;

    private float spawnTimer;

    private Vector3 randomPosition;
    void Start()
    {
        spawnTimer = 0;
    }

    void Update()
    {
        
    }

    private void SpawnLogs()
    {
        
    }

    private void CreateLog()
    {
        var log = Instantiate(logPrefab);
    }

    private Vector3 GetRandomPosition()
    {
        var randomPos = Random.Range(minimumBorder, maximumBorder);

        return randomPosition;
    }
}
