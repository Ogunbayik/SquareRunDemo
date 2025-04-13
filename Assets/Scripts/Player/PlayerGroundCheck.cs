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

    [Header("Check Settings")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius;

    private Grounds grounds;
    private void Awake()
    {
        currentMode = PlayerMode.Normal;
        grounds = GameObject.FindObjectOfType<Grounds>();
    }
    private void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        //USE PHYSÝCS METHOD FOR BOOSTED AREA.
        var checkGround = Physics.CheckSphere(transform.position, checkRadius, groundLayer);

        if (checkGround)
        {
            var playerColor = GetComponentInChildren<SkinnedMeshRenderer>().material.color;
            var currentGround = grounds.GetCurrentGround();
            var groundColor = currentGround.GetComponent<MeshRenderer>().material.color;

            if (playerColor == groundColor)
            {
                ChangeMode(PlayerMode.Boosted);
            }
            else
            {
                ChangeMode(PlayerMode.Decreased);
            }
        }
    }

    private void ChangeMode(PlayerMode mode)
    {
        if(currentMode == mode) { return; }

        currentMode = mode;
    }
}
