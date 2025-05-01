using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Fire Settings")]
    [SerializeField] private GameObject[] magicFiresPrefab;
    [SerializeField] private float minimumSpeed;
    [SerializeField] private float maximumSpeed;
    [Header("Spawn Settings")]
    [SerializeField] private int firstSpawnTime;
    [SerializeField] private int repeatSpawnTime;
    void Start()
    {
        InvokeRepeating(nameof(SpawnRandomFire),firstSpawnTime, repeatSpawnTime);
    }
    private void SpawnRandomFire()
    {
        var randomIndex = Random.Range(0, magicFiresPrefab.Length);
        var randomFire = magicFiresPrefab[randomIndex];
        var fire = Instantiate(randomFire);
        var fireSpeed = GetRandomSpeed();
        fire.transform.position = this.transform.position;
        fire.GetComponent<MagicFire>().SetMagicFire(fireSpeed, this.transform.rotation.eulerAngles);
    }

    private float GetRandomSpeed()
    {
        var randomSpeed = Random.Range(minimumSpeed, maximumSpeed);
        return randomSpeed;
    }
}
