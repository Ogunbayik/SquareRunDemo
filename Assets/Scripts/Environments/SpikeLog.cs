using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpikeLog : MonoBehaviour
{
    public static event Action<SpikeLog,PlayerController> OnSpikeHitPlayer;

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
        SetSpikeRotation(spikeLogRotation);
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
        SpikeMovement(movementDirection);
    }

    private void SetSpikeRotation(Vector3 rotation)
    {
        spikeLogRotation = rotation;
        transform.Rotate(spikeLogRotation);
    }

    public void SpikeMovement(Vector3 direction)
    {
        movementDirection = direction;
        transform.Translate(direction * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            Debug.Log("Hit the Player");
            OnSpikeHitPlayer?.Invoke(this, player);
            Destroy(this.gameObject);
        }
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

    public int GetDecreaseScore()
    {
        return decreasedScore;
    }

}
