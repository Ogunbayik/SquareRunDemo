using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerGroundCheck : MonoBehaviour
{
    public static Action OnPassPhase;
    public enum PlayerMode
    {
        Boosted,
        Normal,
        Decreased
    }

    public PlayerMode currentMode;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float checkRadius;

    private Grounds grounds;
    private void Awake()
    {
        currentMode = PlayerMode.Normal;
        grounds = GameObject.FindObjectOfType<Grounds>();
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    var colorfulGround = collision.gameObject.TryGetComponent<Colorful>(out Colorful colorful);
    //    var colorfulIndex = colorful.GetColorfulGroundIndex();
    //    var phaseIndex = GameManager.Instance.currentPhaseIndex;

    //    if (colorfulGround)
    //    {
    //        if (phaseIndex < colorfulIndex)
    //        {
    //            OnPassPhase?.Invoke();
    //        }
    //    }
    //}
    private void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        //USE PHYSÝCS METHOD FOR BOOSTED AREA.
        var checkGround = Physics.CheckSphere(transform.position, checkRadius, groundLayer);

        if (checkGround)
        {
            var playerColor = GetComponentInChildren<SkinnedMeshRenderer>().material.color;
            var currentGround = grounds.GetCurrentGround();
            var groundColor = currentGround.GetComponent<MeshRenderer>().material.color;

            if (playerColor == groundColor)
            {
                ChangeMode(PlayerMode.Boosted);
            }
            else
            {
                ChangeMode(PlayerMode.Decreased);
            }
        }
    }

    private void ChangeMode(PlayerMode mode)
    {
        if(currentMode == mode) { return; }

        currentMode = mode;
    }
}
