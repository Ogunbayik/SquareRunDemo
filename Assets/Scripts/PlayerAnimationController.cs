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

    public void ActivateSadAnimation(bool isActive)
    {
        playerAnimator.SetBool("isSad", isActive);
    }

    public void ActivateWalkAnimation(bool isActive)
    {
        playerAnimator.SetBool("isWalking", isActive);
    }
    
    public void ActivateRunAnimation(bool isActive)
    {
        playerAnimator.SetBool("isRunning", isActive);
    }
}
