using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameColorManager : MonoBehaviour
{
    public static GameColorManager Instance;

    [Header("Color Settings")]
    [SerializeField] private Color startColor;
    [SerializeField] private Color[] allColors;

    private Color randomColor;
    private void Awake()
    {
        #region Singleton
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
        #endregion
    }
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

    public Color GetStartColor()
    {
        return startColor;
    }
}
