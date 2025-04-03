using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameColorManager : MonoBehaviour
{
    [Header("Color Settings")]
    [SerializeField] private Color[] allColors;

    private Color randomColor;

    public Color GetRandomColor()
    {
        var randomColorIndex = Random.Range(0, allColors.Length);
        randomColor = allColors[randomColorIndex];

        return randomColor;
    }

    public Color[] GetAllColors()
    {
        return allColors;
    }
}
