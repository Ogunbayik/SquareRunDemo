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
    [Header("Aura Settings")]
    [SerializeField] private ParticleSystem boostAuraParticle;
    [SerializeField] private ParticleSystem decreasedAuraParticle;

    private bool isGround;

    private GameObject boostAura;
    private GameObject decreasedAura;
    private void Start()
    {
        currentMode = PlayerMode.Normal;
        boostAura = Instantiate(boostAuraParticle.gameObject,this.transform);
        decreasedAura = Instantiate(decreasedAuraParticle.gameObject, this.transform);
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
        if (isGround)
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
        {
            ChangeMode(PlayerMode.Normal);
            SetPlayerAura();
        }

        //Check ground color when the first collision
        var grounds = collision.gameObject.GetComponentInParent<Grounds>();

        if (grounds != null)
            CheckGroundColor(grounds, this);

    }
    private void OnCollisionStay(Collision collision)
    {
        var ground = collision.gameObject.GetComponentInParent<Grounds>();
        if (ground != null)
            isGround = true;
        else
            isGround = false;
    }
    private void CheckGroundColor(Grounds grounds, PlayerGroundCheck player)
    {
        var playerColor = player.GetComponentInChildren<SkinnedMeshRenderer>().material.color;
        var groundColor = grounds.GetCurrentGround().GetComponent<MeshRenderer>().material.color;

        if(playerColor.r == groundColor.r && playerColor.g == groundColor.g && playerColor.b == groundColor.b)
        {
            ChangeMode(PlayerMode.Boosted);
            SetPlayerAura();
        }
        else
        {
            ChangeMode(PlayerMode.Decreased);
            SetPlayerAura();
        }
    }
    private void SetPlayerAura()
    {
        switch(currentMode)
        {
            case PlayerMode.Normal:
                boostAura.SetActive(false);
                decreasedAura.SetActive(false);
                break;
                    case PlayerMode.Boosted:
                boostAura.SetActive(true);
                decreasedAura.SetActive(false);
                break;
            case PlayerMode.Decreased:
                decreasedAura.SetActive(true);
                boostAura.SetActive(false);
                break;
        }
    }

    private void ChangeMode(PlayerMode mode)
    {
        if(currentMode == mode) { return; }

        currentMode = mode;
    }
}
