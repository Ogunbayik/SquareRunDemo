using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    public List<GameObject> allSpawnedObjects = new List<GameObject>();

    private Grounds grounds;

    [Header("Spawn Prefabs")]
    [SerializeField] private GameObject spikePrefab;
    [SerializeField] private GameObject gemPrefab;
    [Header("Spawn Range")]
    [SerializeField] private float horizontalSpawnRange;
    [SerializeField] private float verticalSpawnRange;
    [Header("Spawn Time")]
    [SerializeField] private float spawnRepeatTime;
    [SerializeField] private Vector3 spikeRotation;
    [SerializeField] private int addRotation;

    private Vector3 randomSpikePosition;
    private Vector3 randomGemPosition;

    private bool canSpawnGem;
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
    private void Update()
    {
        if (canSpawnGem)
            SpawnGem();
    }
    private void OnEnable()
    {
        GameManager.OnGamePhaseStart += GameManager_OnGamePhaseStart;
        GameManager.OnPlayerPassedPhase += GameManager_OnPlayerPassedPhase;
        Gem.OnPlayerCollectedAnyGem += Gem_OnPlayerCollectedAnyGem;
        Gem.OnPlayerCollectGem += Gem_OnPlayerCollectGem;
    }
    private void OnDisable()
    {
        GameManager.OnGamePhaseStart -= GameManager_OnGamePhaseStart;
        GameManager.OnPlayerPassedPhase -= GameManager_OnPlayerPassedPhase;
        Gem.OnPlayerCollectedAnyGem -= Gem_OnPlayerCollectedAnyGem;
        Gem.OnPlayerCollectGem -= Gem_OnPlayerCollectGem;
    }
    private void Gem_OnPlayerCollectGem(Gem gem, PlayerInteraction player)
    {
        allSpawnedObjects.Remove(gem.gameObject);
    }

    private void GameManager_OnPlayerPassedPhase()
    {
        SetCanGemSpawn(false);
        CancelInvoke(nameof(SpawnSpikeInGame));
        ChangeSpikeRotation(addRotation);

        StartCoroutine(nameof(DestroyAllSpawnedObjects));
    }

    private void GameManager_OnGamePhaseStart()
    {
        var firstSpawnTime = 1f;
        InvokeRepeating(nameof(SpawnSpikeInGame), firstSpawnTime, spawnRepeatTime);
        SetCanGemSpawn(true);
    }
    private void Gem_OnPlayerCollectedAnyGem()
    {
        canSpawnGem = true;
    }

    private void SpawnSpikeInGame()
    {
        var spikelog = Instantiate(spikePrefab);
        allSpawnedObjects.Add(spikelog);

        spikelog.transform.position = RandomSpikePosition();
        spikelog.GetComponent<SpikeLog>().SetSpikeRotation(spikeRotation);
    }
    private void ChangeSpikeRotation(int rotation)
    {
        spikeRotation = new Vector3(spikeRotation.x, spikeRotation.y + rotation, spikeRotation.z);
    }
    private Vector3 RandomSpikePosition()
    {
        var spawnDirectionX = grounds.IsDirectionX();

        if (!spawnDirectionX)
        {
            var spawnPoint = grounds.GetSpawnPosition();
            var maximumX = spawnPoint.position.x + horizontalSpawnRange;
            var minimumX = spawnPoint.position.x - horizontalSpawnRange;
            var randomPositionX = Random.Range(minimumX, maximumX);
            randomSpikePosition = new Vector3(randomPositionX, spawnPoint.position.y, spawnPoint.position.z);
        }
        else
        {
            var spawnPoint = grounds.GetSpawnPosition();
            var maximumZ = spawnPoint.position.z + horizontalSpawnRange;
            var minimumZ = spawnPoint.position.z - horizontalSpawnRange;
            var randomPositionZ = Random.Range(minimumZ, maximumZ);
            randomSpikePosition = new Vector3(spawnPoint.position.x, spawnPoint.position.y, randomPositionZ);
        }

        return randomSpikePosition;
    }
    private void SpawnGem()
    {
        var gem = Instantiate(gemPrefab);
        allSpawnedObjects.Add(gem);

        gem.transform.position = RandomGemPosition();
        canSpawnGem = false;
    }
    private Vector3 RandomGemPosition()
    {
        var spawnDirectionX = grounds.IsDirectionX();

        if(!spawnDirectionX)
        {
            var currentGround = grounds.GetCurrentGround();
            var minimumX = currentGround.position.x - horizontalSpawnRange;
            var maximumX = currentGround.position.x + horizontalSpawnRange;
            var minimumZ = currentGround.transform.position.z - verticalSpawnRange;
            var maximumZ = currentGround.transform.position.z + verticalSpawnRange;
            var randomPositionX = Random.Range(minimumX, maximumX);
            var randomPositionZ = Random.Range(minimumZ, maximumZ);
            randomGemPosition = new Vector3(randomPositionX, 0f, randomPositionZ);
        }
        else
        {
            var currentGround = grounds.GetCurrentGround();
            var minimumX = currentGround.position.x - verticalSpawnRange;
            var maximumX = currentGround.position.x + verticalSpawnRange;
            var minimumZ = currentGround.transform.position.z - horizontalSpawnRange;
            var maximumZ = currentGround.transform.position.z + horizontalSpawnRange;
            var randomPositionX = Random.Range(minimumX, maximumX);
            var randomPositionZ = Random.Range(minimumZ, maximumZ);
            randomGemPosition = new Vector3(randomPositionX, 0f, randomPositionZ);
        }

        return randomGemPosition;
    }
    public void SetCanGemSpawn(bool canSpawn)
    {
        canSpawnGem = canSpawn;
    }

    private IEnumerator DestroyAllSpawnedObjects()
    {
        Debug.Log("Destroying is starting");
        yield return new WaitForSeconds(1f);
        DestroyObjects();
        yield return new WaitForSeconds(1f);
        allSpawnedObjects.Clear();
        StopCoroutine(nameof(DestroyAllSpawnedObjects));
    }

    private void DestroyObjects()
    {
        foreach (var spawnedObject in allSpawnedObjects)
        {
            Destroy(spawnedObject.gameObject, 1f);
        }
    }
}
