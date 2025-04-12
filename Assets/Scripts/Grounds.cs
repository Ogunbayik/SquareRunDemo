using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grounds : MonoBehaviour
{
    [SerializeField] private List<Transform> groundList = new List<Transform>();

    [Header("Settings")]
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
    private void OnEnable()
    {
        GameManager.OnTeleported += SetCurrentGround;
    }

    private void OnDisable()
    {
        GameManager.OnTeleported -= SetCurrentGround;
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
        randomTime = Random.Range(minChangeTime, maxChangeTime);
        return randomTime;
    }
}
