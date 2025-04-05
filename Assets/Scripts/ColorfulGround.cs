using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorfulGround : MonoBehaviour
{
    [SerializeField] private float spawnPosition;
    [SerializeField] private Quaternion spawnRotation;
    [SerializeField] private float minimumBorder;
    [SerializeField] private float maximumBorder;

    [SerializeField] private bool directionX;

    private Vector3 randomPosition;
    public Vector3 GetRandomPosition()
    {
        if(directionX)
        {
            var positionZ = Random.Range(minimumBorder, maximumBorder);
            var positionX = spawnPosition;
            randomPosition = new Vector3(positionX, 0f, positionZ);
        }
        else
        {
            var positionX = Random.Range(minimumBorder, maximumBorder);
            var positionZ = spawnPosition;
            randomPosition = new Vector3(positionX, 0f, positionZ);
        }

        return randomPosition;
    }

    public Quaternion GetRotation()
    {
        return spawnRotation;
    }
}
