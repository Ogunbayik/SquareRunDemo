using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGroundCheck : MonoBehaviour
{
    public static Action OnPassPhase;
    public enum PlayerMode
    {
        Boosted,
        Normal,
        Decreased
    }

    [HideInInspector]
    public PlayerMode currentMode;

    private SkinnedMeshRenderer skinnedMeshRenderer;
    private void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        
    }
    private void Start()
    {
        currentMode = PlayerMode.Normal;
    }
    private void OnEnable()
    {
        Grounds.OnGroundColorChange += Grounds_OnGroundColorChange;
    }

    private void OnDisable()
    {
        Grounds.OnGroundColorChange -= Grounds_OnGroundColorChange;
    }
    private void Grounds_OnGroundColorChange(Grounds ground)
    {
        CheckPlayerColor(ground);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Need to check first time when player collision with grounds.
    }

    private void CheckPlayerColor(Grounds ground)
    {
        var playerColor = skinnedMeshRenderer.material.color;
        var groundColor = ground.GetCurrentGround().GetComponent<MeshRenderer>().material.color;

        if(playerColor == groundColor)
        {
            ChangeMode(PlayerMode.Boosted);
            Debug.Log("Player is boosted mode.");
        }
        else
        {
            ChangeMode(PlayerMode.Decreased);
            Debug.Log("Player is Decreased mode.");
        }
    }
    private void ChangeMode(PlayerMode mode)
    {
        if(currentMode == mode) { return; }

        currentMode = mode;
    }
}
