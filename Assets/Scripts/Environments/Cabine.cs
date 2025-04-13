using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cabine : MonoBehaviour, IInteractable
{
    [Header("Color Settings")]
    [SerializeField] private Color cabineColor;

    private GameColorManager colorManager;

    private MeshRenderer cabineRenderer;

    private void Awake()
    {
        colorManager = FindObjectOfType<GameColorManager>();
    }
    private void Start()
    {
        cabineRenderer = GetComponent<MeshRenderer>();
        cabineRenderer.material.color = cabineColor;
    }

    public void Interact(PlayerInteract player)
    {
        var playerSkinnedMeshRenderer = player.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        var playerColor = playerSkinnedMeshRenderer.material.color;

        if (playerColor == cabineColor)
            return;

        playerSkinnedMeshRenderer.material.color = cabineColor;
    }

}
