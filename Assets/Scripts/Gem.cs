using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gem : MonoBehaviour
{
    public static Action<int> OnGemCollected;

    private Animator animator;

    [Header("Score Settings")]
    [SerializeField] private int gemScore;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            OnGemCollected?.Invoke(gemScore);
            animator.SetTrigger(Consts.GemAnimationParameter.COLLECT_PARAMETER);
            SelfDestruction();
        }
    }

    private void SelfDestruction()
    {
        var destructionTime = 0.3f;
        Destroy(this.gameObject, destructionTime);
    }

}
