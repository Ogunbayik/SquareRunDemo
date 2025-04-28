using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimationController : MonoBehaviour
{
    private Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnEnable()
    {
        Ghost.OnGhostStateChanged += Ghost_OnGhostStateChanged;
    }
    private void OnDisable()
    {
        Ghost.OnGhostStateChanged -= Ghost_OnGhostStateChanged;
    }
    private void Ghost_OnGhostStateChanged(GhostStates currentState, GhostStates newState)
    {
        animator.SetBool(Consts.GhostAnimationParameter.MOVING_PARAMETER, newState == GhostStates.Move);
    }


}
