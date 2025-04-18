using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, ICollectable
{
    public static event Action<Key> OnPlayerCollectedKey;

    [Header("Settings")]
    [SerializeField] private int keyID;

    public void Collect(PlayerInteraction player)
    {
        OnPlayerCollectedKey?.Invoke(this);
    }

    public int GetKeyID()
    {
        return keyID;
    }
}