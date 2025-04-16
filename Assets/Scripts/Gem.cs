using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gem : MonoBehaviour
{
    public static event Action OnPlayerCollectedAnyGem;
    public static event Action<Gem,PlayerController> OnPlayerCollectGem;

    private Animator animator;

    [Header("Score Settings")]
    [SerializeField] private int gemScore;
    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
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
        animator.SetTrigger(Consts.GemAnimationParameter.COLLECT_PARAMETER);
        SelfDestruction();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            OnPlayerCollectGem?.Invoke(this, player);
            OnPlayerCollectedAnyGem?.Invoke();
        }
    }

    private void SelfDestruction()
    {
        var destructionTime = 0.3f;
        Destroy(this.gameObject, destructionTime);
    }

    public int GetGemScore()
    {
        return gemScore;
    }



}
