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

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask colorfulLayer;
    [SerializeField] private float checkRadius;
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

    private void Update()
    {
        var checkGround = Physics.CheckSphere(transform.position, checkRadius, groundLayer);
        var checkColorful = Physics.CheckSphere(transform.position, checkRadius, colorfulLayer);

        if (checkGround)
        {
            ChangeMode(PlayerMode.Boosted);
            Debug.Log("Player can get boost in this area.");
        }
        else if (checkColorful)
        {
            //Change Game Mode in this area.
            Debug.Log("Player in colorful area.");
        }
    }


    private void ChangeMode(PlayerMode mode)
    {
        if(currentMode == mode) { return; }

        currentMode = mode;
    }
}
