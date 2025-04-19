using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseStartTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private int doorID;

    public int GetDoorID()
    {
        return doorID;
    }
}
