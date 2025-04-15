using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public static event Action<PlayerController,SpikeLog> OnPlayerHitSpike;
    void Start()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void OnPlayerHittedSpike(PlayerController player, SpikeLog spike)
    {
        OnPlayerHitSpike?.Invoke(player, spike);
    }



}
