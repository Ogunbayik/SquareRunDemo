using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colorful : MonoBehaviour
{
    private int colorfulGroundIndex;
    public void SetColorfulGroundIndex(int index)
    {
        colorfulGroundIndex = index;
    }

    public int GetColorfulGroundIndex()
    {
        return colorfulGroundIndex;
    }
}
