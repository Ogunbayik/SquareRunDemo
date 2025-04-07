using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpikeLog : MonoBehaviour
{
    public static Action<int> OnHitPlayer;

    private GameColorManager colorManager;

    private Material[] spikeMaterials;

    [SerializeField] private float movementSpeed;
    [SerializeField] private int minDecreaseScore;
    [SerializeField] private int maxDecreaseScore;

    private int decreasedScore;
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

    private void SetRandomDecreaseScore()
    {
        decreasedScore = UnityEngine.Random.Range(minDecreaseScore, maxDecreaseScore);
    }

    private void Update()
    {
        transform.Translate(transform.TransformDirection(transform.forward) * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            Debug.Log("Hit the Player");
            OnHitPlayer?.Invoke(decreasedScore);
        }
    }

}
