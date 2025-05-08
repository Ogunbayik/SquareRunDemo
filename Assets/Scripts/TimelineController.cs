using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;
using System;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector playableDirector;
    [Header("Camera Settings")]
    public CinemachineVirtualCamera gameCamera;
    public CinemachineVirtualCamera cutsceneCamera;
    [Header("Scene Settings")]
    //[SerializeField] private PlayableAsset sceneIntro;
    [SerializeField] private PlayableAsset sceneCloseDoor;
    private void OnEnable()
    {
        GameManager.OnGamePhaseStart += GameManager_OnGamePhaseStart;
    }
    private void OnDisable()
    {
        GameManager.OnGamePhaseStart -= GameManager_OnGamePhaseStart;
    }
    private void GameManager_OnGamePhaseStart()
    {
        StartCoroutine(nameof(EnableCloseDoor));
    }
    private IEnumerator EnableCloseDoorCutscene()
    {
        InitializeDoorSettings();

        yield return new WaitForSeconds(0.5f);

        EnableCloseDoor();

    }
    private void InitializeDoorSettings()
    {
        cutsceneCamera.transform.position = gameCamera.transform.position;
    }
    private void EnableCloseDoor()
    {
        playableDirector.Play(sceneCloseDoor);
    }

    private void SetCutsceneCameraPosition()
    {
        cutsceneCamera.transform.position = gameCamera.transform.position;
    }
}
