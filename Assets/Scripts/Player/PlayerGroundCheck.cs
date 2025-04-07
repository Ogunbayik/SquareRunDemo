using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundCheck : MonoBehaviour
{
    public enum PlayerMode
    {
        Boosted,
        Normal,
        Decreased
    }

    public PlayerMode currentMode;
    private void Awake()
    {
        currentMode = PlayerMode.Normal;
    }
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
                ChangeMode(PlayerMode.Boosted);
                Debug.Log("Player can get boost in this area.");
            }
            else
            {
                ChangeMode(PlayerMode.Decreased);
                Debug.Log("Player decrease that it's power");
            }
        }
    }

    private void ChangeMode(PlayerMode mode)
    {
        if(currentMode == mode) { return; }

        currentMode = mode;
    }
}
