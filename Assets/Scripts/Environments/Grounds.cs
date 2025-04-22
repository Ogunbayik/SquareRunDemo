using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Grounds : MonoBehaviour
{
    public static event Action<Grounds> OnGroundColorChange;

    [SerializeField] private List<Transform> groundList = new List<Transform>();

    [Header("Timer Settings")]
    [SerializeField] private float minChangeTime;
    [SerializeField] private float maxChangeTime;

    private bool xDirection;

    private Transform currentGround;
    private Transform spawnPosition;

    private float randomTime;
    void Start()
    {
        SetupGrounds();
        GetRandomTime();
    }

    private void Update()
    {
        AllGroundsColorChange();
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerTeleportNextPhase += PlayerController_OnPlayerTeleportNextPhase;
    }
    private void OnDisable()
    {
        PlayerController.OnPlayerTeleportNextPhase -= PlayerController_OnPlayerTeleportNextPhase;
    }
    private void PlayerController_OnPlayerTeleportNextPhase()
    {
        SetCurrentGround();
    }

    private void SetupGrounds()
    {
        var allGrounds = gameObject.GetComponentsInChildren<Transform>();

        foreach (var ground in allGrounds)
        {
            groundList.Add(ground);
            groundList.Remove(this.transform);

            if(ground.GetComponent<MeshRenderer>() == null)
            {
                groundList.Remove(ground);
            }
        }

        for (int i = 0; i < groundList.Count; i++)
        {
            groundList[i].gameObject.GetComponent<MeshRenderer>().material.color = GameColorManager.Instance.GetStartColor();
            xDirection = false;
        }

        currentGround = groundList[0];
        spawnPosition = currentGround.GetChild(0);
    }

    private void AllGroundsColorChange()
    {
        randomTime -= Time.deltaTime;

        if(randomTime <= 0)
        {
            foreach (var ground in groundList)
            {
                //All grounds are getting random color.
                ground.gameObject.GetComponent<MeshRenderer>().material.color = GameColorManager.Instance.GetRandomColor();
            }
            //Reset timer
            OnGroundColorChange?.Invoke(this);
            randomTime = GetRandomTime();
        }
    }

    private void SetCurrentGround()
    {
        var phaseIndex = GameManager.Instance.GetPhaseIndex();
        currentGround = groundList[phaseIndex];
        xDirection = !xDirection;
    }

    public Transform GetCurrentGround()
    {
        return currentGround;
    }

    public Transform GetSpawnPosition()
    {
        spawnPosition = currentGround.GetChild(0);
        return spawnPosition;
    }

    public bool IsDirectionX()
    {
        return xDirection;
    }

    private float GetRandomTime()
    {
        randomTime = UnityEngine.Random.Range(minChangeTime, maxChangeTime);
        return randomTime;
    }
}
