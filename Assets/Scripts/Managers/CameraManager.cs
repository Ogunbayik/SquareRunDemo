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

    private Vector3 newCameraRotation;

    private bool canRotate;
    private void OnEnable()
    {
        PlayerController.OnPlayerTeleportNextPhase += PlayerController_OnPlayerTeleportNextPhase;
    }
    private void OnDisable()
    {
        PlayerController.OnPlayerTeleportNextPhase -= PlayerController_OnPlayerTeleportNextPhase;
    }

    private void PlayerController_OnPlayerTeleportNextPhase()
    {
        SetCameraRotation();
        canRotate = true;
    }
    private void Update()
    {
        if (canRotate)
            followCamera.transform.rotation = Quaternion.Slerp(followCamera.transform.rotation, Quaternion.Euler(newCameraRotation), 1f * Time.deltaTime);

        if (followCamera.transform.rotation == Quaternion.Euler(newCameraRotation))
            canRotate = false;
    }
    private void SetCameraRotation()
    {
        //Camera Rotation Only Y Coordinate.
        var followCameraRotation = followCamera.transform.eulerAngles;
        newCameraRotation = followCameraRotation + new Vector3(0f, addRotationY, 0f);
    }
}
