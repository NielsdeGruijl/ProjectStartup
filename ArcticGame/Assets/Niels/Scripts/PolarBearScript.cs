using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PolarBearScript : MonoBehaviour
{
    private CharacterMovement player;
    private Rigidbody rb;

    private Vector3 velocity;

    private float xVelocity;
    private float zVelocity;
    private float speed;
    

    private void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        if (transform.childCount > 0 && player == null)
        {
            player = GetComponentInChildren<CharacterMovement>();
        }

        if (player != null && speed == 0)
        {
            speed = player.movementSpeed * 2;
        }

        if (player != null)
        {
            if (player.mounted)
            {
                movement();
            }
        }
    }

    void movement()
    {
        xVelocity = Input.GetAxisRaw("Horizontal");
        zVelocity = Input.GetAxisRaw("Vertical");

        velocity = new Vector3(xVelocity, 0, zVelocity);
        velocity = velocity.normalized;
    }

    private void FixedUpdate()
    {
        rb.AddForce(velocity * speed);
    }
}
