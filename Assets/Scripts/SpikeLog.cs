using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpikeLog : MonoBehaviour
{
    public static Action OnTriggerPlayer;

    private GameColorManager colorManager;

    private Material[] spikeMaterials;

    [SerializeField] private float movementSpeed;
    private void Awake()
    {
        colorManager = FindObjectOfType<GameColorManager>();
    }
    void Start()
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
        transform.Translate(transform.TransformDirection(transform.forward) * movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            Debug.Log("Hit the Player");
            OnTriggerPlayer?.Invoke();
        }
    }

}
