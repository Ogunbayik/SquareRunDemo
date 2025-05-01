using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFire : MonoBehaviour, IHitable
{
    [Header("Particle Settings")]
    [SerializeField] private ParticleSystem explosionParticle;
    [Header("General Settings")]
    [SerializeField] private int decreaseScore;

    private float movementSpeed;
    private Vector3 movementDirection;
    private Vector3 fireRotation;
    private void Start()
    {
        transform.rotation = Quaternion.Euler(fireRotation);
    }
    void Update()
    {
        Movement();
    }
    private void Movement()
    {
        movementDirection = Vector3.forward;
        transform.Translate(movementDirection * movementSpeed * Time.deltaTime);
    }
    public void SetMagicFire(float speed, Vector3 rotation)
    {
        movementSpeed = speed;
        fireRotation = rotation;
    }

    public void HitPlayer(PlayerInteraction player)
    {
        Destroy(this.gameObject);

        var explosion = Instantiate(explosionParticle);
        var explosionDestroyTime = 1f;
        explosion.transform.position = player.transform.position;
        Destroy(explosion.gameObject, explosionDestroyTime);
    }

    public int GetDecreaseScore()
    {
        return decreaseScore;
    }
}
