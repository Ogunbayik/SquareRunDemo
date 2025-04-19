using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, ICollectable
{
    public static event Action<Key> OnPlayerCollectedKey;

    [Header("Settings")]
    [SerializeField] private int keyID;
    [SerializeField] private float selfDestructionTime;

    public void Collect(PlayerInteraction player)
    {
        OnPlayerCollectedKey?.Invoke(this);
    }
    private void OnEnable()
    {
        OnPlayerCollectedKey += SelfDestruction;
    }
    private void OnDisable()
    {
        OnPlayerCollectedKey += SelfDestruction;
    }

    private void SelfDestruction(Key key)
    {
        Destroy(key.gameObject, selfDestructionTime);
    }

    public int GetKeyID()
    {
        return keyID;
    }
}