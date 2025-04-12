using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    private Grounds grounds;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private float maxSpawnTimer;
    [SerializeField] private float spawnRange;

    private Vector3 spikeRotation;
    private Vector3 randomPosition;

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

        grounds = GameObject.FindObjectOfType<Grounds>();
    }
    void Start()
    {
        ResetSpawning();
        SetSpikeRotation(spikeRotation);
    }

    void Update()
    {
        SpawnActivation();
    }
    private void SpawnActivation()
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
            var spikeLog = Instantiate(spikePrefab);
            var movementDirection = spikeLog.transform.forward;

            spikeLog.transform.position = RandomSpikePosition();
            Debug.Log(RandomSpikePosition());
            spikeLog.transform.Rotate(GetSpikeRotation());
            spikeLog.GetComponent<SpikeLog>().SpikeMovement(movementDirection);
            spawnTimer = maxSpawnTimer;
        }
    }

    private Vector3 RandomSpikePosition()
    {
        var spawnDirectionX = grounds.IsDirectionX();

        if (!spawnDirectionX)
        {
            var spawnPoint = grounds.GetSpawnPosition();
            var maximumX = spawnPoint.position.x + spawnRange;
            var minimumX = spawnPoint.position.x - spawnRange;
            var randomPositionX = Random.Range(minimumX, maximumX);
            randomPosition = new Vector3(randomPositionX, spawnPoint.position.y, spawnPoint.position.z);
        }
        else
        {
            var spawnPoint = grounds.GetSpawnPosition();
            var maximumZ = spawnPoint.position.z + spawnRange;
            var minimumZ = spawnPoint.position.z - spawnRange;
            var randomPositionZ = Random.Range(minimumZ, maximumZ);
            randomPosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, randomPositionZ);
        }

        return randomPosition;
    }

    public void SetSpikeRotation(Vector3 desiredRotation)
    {
        spikeRotation = desiredRotation;
    }

    public Vector3 GetSpikeRotation()
    {
        return spikeRotation;
    }

    public void ResetSpawning()
    {
        spawnTimer = maxSpawnTimer;
    }
}
