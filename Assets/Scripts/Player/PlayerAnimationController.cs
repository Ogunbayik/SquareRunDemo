using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator playerAnimator;
    private void Awake()
    {
        playerAnimator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        GameManager.OnTeleport += ActvateTeleportAnimation;
    }
    private void OnDisable()
    {
        GameManager.OnTeleport -= ActvateTeleportAnimation;
    }

    public void ActivateSadAnimation(bool isActive)
    {
        playerAnimator.SetBool(Consts.PlayerAnimationParameter.SAD_PARAMETER, isActive);
    }

    public void ActivateWalkAnimation(bool isActive)
    {
        playerAnimator.SetBool(Consts.PlayerAnimationParameter.WALK_PARAMETER, isActive);
    }
    
    public void ActivateRunAnimation(bool isActive)
    {
        playerAnimator.SetBool(Consts.PlayerAnimationParameter.RUN_PARAMETER, isActive);
    }

    private void ActvateTeleportAnimation()
    {
        playerAnimator.SetBool(Consts.PlayerAnimationParameter.TELEPORT_PARAMETER, true);
    }

    public void ResetTeleportAnimation()
    {
        playerAnimator.SetBool(Consts.PlayerAnimationParameter.TELEPORT_PARAMETER, false);
        //After teleport character can move
        GameManager.Instance.ChangeState(GameManager.GameStates.GameStart);
    }
}
