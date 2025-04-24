using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Cabine : MonoBehaviour, IInteractable
{
    public static event Action<PlayerInteraction,Cabine> OnPlayerColorChanged;

    [Header("Color Settings")]
    [SerializeField] private Color cabineColor;
    [SerializeField] private Transform cabineFloor;
    [Header("Alpha Settings")]
    [SerializeField] private float minimumAlphaIndex;
    [SerializeField] private float maximumAlphaIndex;

    private Cabine currentCabine;
    private MeshRenderer cabineRenderer;

    private float alphaIndex;

    private bool isPlayerInside;
    private void Start()
    {
        alphaIndex = maximumAlphaIndex;
        cabineRenderer = GetComponent<MeshRenderer>();
        cabineRenderer.material.color = cabineColor;
        cabineFloor.GetComponent<MeshRenderer>().material.color = cabineColor;
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
    private void Update()
    {
        if (isPlayerInside)
            DecreaseAlpha();
        else
            IncreaseAlpha();
    }
    private void DecreaseAlpha()
    {
        if (alphaIndex <= minimumAlphaIndex)
            alphaIndex = minimumAlphaIndex;
        else
            alphaIndex -= Time.deltaTime;

        var cabineColor = currentCabine.GetComponent<MeshRenderer>().material.color;
        cabineColor.a = alphaIndex;
        currentCabine.GetComponent<MeshRenderer>().material.color = cabineColor;
    }
    private void IncreaseAlpha()
    {
        if (alphaIndex == maximumAlphaIndex)
            return;

        if (alphaIndex >= maximumAlphaIndex)
            alphaIndex = maximumAlphaIndex;
        else
            alphaIndex += Time.deltaTime;

        var cabineColor = this.GetComponent<MeshRenderer>().material.color;
        cabineColor.a = alphaIndex;
        this.GetComponent<MeshRenderer>().material.color = cabineColor;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerInteraction>())
        {
            currentCabine = this; 
            isPlayerInside = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.GetComponent<PlayerInteraction>())
        {
            isPlayerInside = false;
            currentCabine = null;
        }
    }


}
