using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColorCheck : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Ground>(out Ground ground))
        {
            var groundColor = ground.GetComponent<MeshRenderer>().material.color;
            var playerColor = GetComponentInChildren<SkinnedMeshRenderer>().material.color;

            if (groundColor == playerColor)
            {
                //Player's score power can multiply in this area.
                Debug.Log("Player can get boost in this area.");
            }
            else
            {
                //Player's run stamina decrease in this area.
                Debug.Log("Player decrease that it's power");
            }
        }
    }
}
