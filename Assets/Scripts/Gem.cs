using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gem : MonoBehaviour
{
    public static Action<int> OnGemCollected;

    [Header("Settings")]
    [SerializeField] private int gemScore;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            OnGemCollected?.Invoke(gemScore);
        }
    }


}
