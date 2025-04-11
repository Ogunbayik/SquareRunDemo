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

    private Vector3 spikeRotation;

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

            spikeLog.transform.position = grounds.GetSpawnPosition().position;
            spikeLog.transform.Rotate(GetSpikeRotation());
            spikeLog.GetComponent<SpikeLog>().SpikeMovement(movementDirection);
            spawnTimer = maxSpawnTimer;
        }
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
