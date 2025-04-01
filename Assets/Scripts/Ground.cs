using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private MeshRenderer groundMeshRenderer;

    [SerializeField] private Color[] allColors;
    [SerializeField] private float maxChangeTimer;
    [SerializeField] private float minChangeTimer;

    private float changeTimer;
    private void Awake()
    {
        groundMeshRenderer = GetComponent<MeshRenderer>();
        GetRandomTime();
    }
    void Start()
    {
        GetRandomTime();
        SetRandomColor();
    }

    private void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        changeTimer -= Time.deltaTime;

        if (changeTimer <= 0)
        {
            SetRandomColor();
            GetRandomTime();
        }
    }

    private void SetRandomColor()
    {
        var randomIndex = Random.Range(0, allColors.Length);
        var newRandomColor = allColors[randomIndex];

        groundMeshRenderer.material.color = newRandomColor;
    }

    private float GetRandomTime()
    {
        changeTimer = Random.Range(minChangeTimer, maxChangeTimer);
        return changeTimer;
    }
}
