using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpikeLog : MonoBehaviour, IHitable
{
    private GameColorManager colorManager;

    private Material[] spikeMaterials;

    [Header("Settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private int minDecreaseScore;
    [SerializeField] private int maxDecreaseScore;

    private int decreasedScore;

    private Vector3 movementDirection;
    private Vector3 spikeLogRotation;
    private void Awake()
    {
        colorManager = FindObjectOfType<GameColorManager>();
    }
    void Start()
    {
        SetRandomColor();
        SetRandomDecreaseScore();
    }
    private void SetRandomColor()
    {
        spikeMaterials = gameObject.GetComponentInChildren<MeshRenderer>().materials;

        var cylinderIndex = 0;
        var spikeIndex = 1;
        var randomColor = colorManager.GetRandomColor();

        for (int i = 0; i < spikeMaterials.Length; i++)
        {
            spikeMaterials[cylinderIndex].color = randomColor;
            spikeMaterials[spikeIndex].color = Color.black;
        }
    }
    private void Update()
    {
        SpikeMovement();
    }

    public void SetSpikeRotation(Vector3 rotation)
    {
        spikeLogRotation = rotation;
        transform.Rotate(spikeLogRotation);
    }

    private void SpikeMovement()
    {
        movementDirection = Vector3.forward;
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime, Space.Self);
    }
    private void SetRandomDecreaseScore()
    {
        decreasedScore = UnityEngine.Random.Range(minDecreaseScore, maxDecreaseScore);
    }

    public Color GetSpikeColor()
    {
        var cylinderIndex = 0;
        var spikeColor = spikeMaterials[cylinderIndex].color;

        return spikeColor;
    }

    public void HitPlayer(PlayerInteraction player)
    {
        Debug.Log($"{gameObject.name} hitted the player.");
        SpawnManager.Instance.allSpawnedObjects.Remove(this.gameObject);
        Destroy(this.gameObject);

        var playerColor = player.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
        var cylinderIndex = 0;
        var spikeColor = spikeMaterials[cylinderIndex].color;
        var decreaseMultiply = 2;

        if (playerColor.r != spikeColor.r || playerColor.g != spikeColor.g || playerColor.b != spikeColor.b)
            decreasedScore *= decreaseMultiply;

        ScoreManager.Instance.DecreaseGameScore(decreasedScore);
    }
}
