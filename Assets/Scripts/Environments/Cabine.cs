using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cabine : MonoBehaviour, IInteractable
{
    public static event Action<PlayerInteraction,Cabine> OnPlayerColorChanged;

    [Header("Color Settings")]
    [SerializeField] private Color cabineColor;

    private MeshRenderer cabineRenderer;

    private void Start()
    {
        cabineRenderer = GetComponent<MeshRenderer>();
        cabineRenderer.material.color = cabineColor;
    }

    public void Interact(PlayerInteraction player)
    {
        var playerSkinnedMeshRenderer = player.gameObject.GetComponentInChildren<SkinnedMeshRenderer>();
        var playerColor = playerSkinnedMeshRenderer.material.color;

        if (playerColor == cabineColor)
            return;

        playerSkinnedMeshRenderer.material.color = cabineColor;
        OnPlayerColorChanged?.Invoke(player, this);
    }

}
