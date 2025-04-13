using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorfulGrounds : MonoBehaviour
{
    private List<Transform> colorfulGroundList = new List<Transform>();

    private Transform nextColorfulGround;
    private void Start()
    {
        var allColorfulGrounds = GetComponentsInChildren<Transform>();

        foreach (var colorfulGround in allColorfulGrounds)
        {
            colorfulGroundList.Add(colorfulGround);
            colorfulGroundList.Remove(this.transform);
        }

        var phaseIndex = GameManager.Instance.GetPhaseIndex();
        nextColorfulGround = colorfulGroundList[phaseIndex + 1];
    }
    private void Update()
    {
        //NEED TO CHANGE UPDATE MODE.. (LATER)
        SetNextColorfulGround();
    }
    private void SetNextColorfulGround()
    {
        var phaseIndex = GameManager.Instance.GetPhaseIndex();
        if (phaseIndex < colorfulGroundList.Count)
            nextColorfulGround = colorfulGroundList[phaseIndex + 1];
        else
            nextColorfulGround = null;
    }

    public Transform GetNextColorfulGround()
    {
        return nextColorfulGround;
    }
}
