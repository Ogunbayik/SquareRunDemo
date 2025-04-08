using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorfulGrounds : MonoBehaviour
{
    [SerializeField] private List<Transform> colorfulGroundList = new List<Transform>();

    private void Start()
    {
        var allColorfulGrounds = GetComponentsInChildren<Transform>();

        foreach (var colorfulGround in allColorfulGrounds)
        {
            colorfulGroundList.Add(colorfulGround);
            colorfulGroundList.Remove(this.transform);
        }

        for (int i = 0; i < colorfulGroundList.Count; i++)
        {
            colorfulGroundList[i].GetComponent<Colorful>().SetColorfulGroundIndex(i);
        }
    }
}
