using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounds : MonoBehaviour
{
    private List<Transform> groundList = new List<Transform>();

    [Header("Settings")]
    [SerializeField] private float minChangeTime;
    [SerializeField] private float maxChangeTime;

    private Transform currentGround;

    private float randomTime;
    void Start()
    {
        SetupGrounds();
        GetRandomTime();
    }

    private void Update()
    {
        ChangeGroundColor();
    }

    private void SetupGrounds()
    {
        var allGrounds = gameObject.GetComponentsInChildren<Transform>();
        foreach (var ground in allGrounds)
        {
            groundList.Add(ground);
            groundList.Remove(this.transform);
        }

        for (int i = 0; i < groundList.Count; i++)
        {
            groundList[i].gameObject.GetComponent<MeshRenderer>().material.color = GameColorManager.Instance.GetStartColor();
            SetCurrentGround();
        }
    }

    private void ChangeGroundColor()
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
            randomTime = GetRandomTime();
        }
    }

    private void SetCurrentGround()
    {
        var phaseIndex = GameManager.Instance.currentPhaseIndex;
        currentGround = groundList[phaseIndex];
    }

    public Transform GetCurrentGround()
    {
        return currentGround;
    }

    private float GetRandomTime()
    {
        randomTime = Random.Range(minChangeTime, maxChangeTime);
        return randomTime;
    }
}
