using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpikeLog : MonoBehaviour
{
    public static Action<int> OnHitPlayer;

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
        GetRandomDecreaseScore();
    }
    private void OnEnable()
    {
        EventManager.OnPlayerHitSpike += EventManager_OnPlayerHitSpike;
    }

    private void EventManager_OnPlayerHitSpike(PlayerController player, SpikeLog spike)
    {
        var playerColor = player.GetPlayerColor();
        if(playerColor == spikeMaterials[0].color)
        {
            Debug.Log("Player and Spike colors are same");
        }
        else
        {
            Debug.Log("Player and Spike are not same Color!!");
        }
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
            OnHitPlayer?.Invoke(decreasedScore);
            EventManager_OnPlayerHitSpike(player, this);
        }
    }
    private int GetRandomDecreaseScore()
    {
        decreasedScore = UnityEngine.Random.Range(minDecreaseScore, maxDecreaseScore);
        return decreasedScore;
    }

}
