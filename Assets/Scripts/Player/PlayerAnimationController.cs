using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        PlayerStateManager.OnStateChanged += HandleStateChanged;
    }
    private void OnDisable()
    {
        PlayerStateManager.OnStateChanged -= HandleStateChanged;
    }

    private void HandleStateChanged(PlayerStateManager.PlayerStates previousState, PlayerStateManager.PlayerStates newState)
    {
        animator.SetBool(Consts.PlayerAnimationParameter.WALK_PARAMETER, newState == PlayerStateManager.PlayerStates.Walking);
        animator.SetBool(Consts.PlayerAnimationParameter.RUN_PARAMETER, newState == PlayerStateManager.PlayerStates.Running);
    }

    public void ActivateSadAnimation(bool isActive)
    {
        animator.SetBool(Consts.PlayerAnimationParameter.SAD_PARAMETER, isActive);
    }
    public void ActivateTeleportAnimation()
    {
        animator.SetBool(Consts.PlayerAnimationParameter.TELEPORT_PARAMETER, true);
    }

    public void ResetTeleportAnimation()
    {
        animator.SetBool(Consts.PlayerAnimationParameter.TELEPORT_PARAMETER, false);
    }
}
