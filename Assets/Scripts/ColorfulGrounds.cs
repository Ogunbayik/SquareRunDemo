using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorfulGrounds : MonoBehaviour
{
    [SerializeField] private List<Transform> colorfulGroundList = new List<Transform>();

    private Transform nextColorfulGround;

    private void Start()
    {
        var allColorfulGrounds = GetComponentsInChildren<Transform>();

        foreach (var colorfulGround in allColorfulGrounds)
        {
            colorfulGroundList.Add(colorfulGround);
            colorfulGroundList.Remove(this.transform);
        }

        var currentIndex = GameManager.Instance.currentPhaseIndex;
        nextColorfulGround = colorfulGroundList[currentIndex + 1];
    }

    public Transform GetNextColorfulGround()
    {
        return nextColorfulGround;
    }
}
