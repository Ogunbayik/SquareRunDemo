using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGroundCheck : MonoBehaviour
{
    public enum PlayerMode
    {
        Boosted,
        Normal,
        Decreased
    }

    [HideInInspector]
    public PlayerMode currentMode;
    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem boostAura;
    [SerializeField] private ParticleSystem decreasedAura;
    private void Start()
    {
        currentMode = PlayerMode.Normal;
    }
    private void OnEnable()
    {
        Grounds.OnGroundColorChange += Grounds_OnGroundColorChange;
        Cabine.OnPlayerColorChanged += Cabine_OnPlayerColorChanged;
    }
    private void OnDisable()
    {
        Grounds.OnGroundColorChange -= Grounds_OnGroundColorChange;
        Cabine.OnPlayerColorChanged -= Cabine_OnPlayerColorChanged;
    }
    private void Grounds_OnGroundColorChange(Grounds ground)
    {
        CheckGroundColor(ground, this);
    }
    private void Cabine_OnPlayerColorChanged(PlayerInteraction player, Cabine cabine)
    {
        var grounds = FindObjectOfType<Grounds>();

        CheckGroundColor(grounds, this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        var colorfulGround = collision.gameObject.GetComponentInParent<ColorfulGrounds>();
        if (colorfulGround != null)
            ChangeMode(PlayerMode.Normal);

        //Check ground color when the first collision
        var grounds = collision.gameObject.GetComponentInParent<Grounds>();

        if (grounds != null)
            CheckGroundColor(grounds, this);

    }
    private void CheckGroundColor(Grounds grounds, PlayerGroundCheck player)
    {
        var playerColor = player.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
        var groundColor = grounds.GetCurrentGround().GetComponent<MeshRenderer>().material.color;

        if(playerColor.r == groundColor.r && playerColor.g == groundColor.g && playerColor.b == groundColor.b)
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
