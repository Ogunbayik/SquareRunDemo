using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gem : MonoBehaviour, ICollectable
{
    public static event Action OnPlayerCollectedAnyGem;
    public static event Action<Gem,PlayerInteraction> OnPlayerCollectGem;

    private Animator animator;
    private Collider gemCollider;

    [Header("Score Settings")]
    [SerializeField] private int gemScore;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        gemCollider = GetComponent<Collider>();
    }
    private void OnEnable()
    {
        OnPlayerCollectedAnyGem += Gem_OnPlayerCollectedAnyGem;
    }
    private void OnDisable()
    {
        OnPlayerCollectedAnyGem -= Gem_OnPlayerCollectedAnyGem;
    }

    private void Gem_OnPlayerCollectedAnyGem()
    {
        gemCollider.enabled = false;
        animator.SetTrigger(Consts.GemAnimationParameter.COLLECT_PARAMETER);
        SelfDestruction();
    }
    public void SelfDestruction()
    {
        var destructionTime = 2f;
        Destroy(this.gameObject, destructionTime);
    }
    public void Collect(PlayerInteraction player)
    {
        OnPlayerCollectedAnyGem?.Invoke();
        OnPlayerCollectGem?.Invoke(this, player);
    }
    public int GetGemScore()
    {
        return gemScore;
    }
}
