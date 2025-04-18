using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private GameObject followCamera;
    [SerializeField] private int addRotationY;
    private void OnEnable()
    {
        GameManager.OnTeleported += SetCameraRotation;
    }
    private void OnDisable()
    {
        GameManager.OnTeleported -= SetCameraRotation;
    }
    private void SetCameraRotation()
    {
        //Camera Rotation Only Y Coordinate.
        var newCameraRotation = new Vector3(0f, addRotationY, 0f);
        followCamera.transform.Rotate(newCameraRotation);
    }
}
