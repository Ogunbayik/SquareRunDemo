using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<ColorfulGround>(out ColorfulGround colorfulGround))
        {
            GameManager.Instance.SetCurrentColorfulGround(colorfulGround);
            SpawnManager.Instance.SetCurrentColorfulGround(colorfulGround);
        }
    }


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
