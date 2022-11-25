using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private Rigidbody rb;

    private Vector3 playerVelocity;

    public float movementSpeed;

    private float xVelocity;
    private float zVelocity;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        Walking();
    }

    private void Walking()
    {
        xVelocity = Input.GetAxisRaw("Horizontal");
        zVelocity = Input.GetAxisRaw("Vertical");

        playerVelocity = new Vector3(xVelocity, 0, zVelocity);
        playerVelocity = playerVelocity.normalized;

        transform.LookAt(new Vector3(transform.localPosition.x + playerVelocity.x, transform.position.y, transform.localPosition.z + playerVelocity.z));
    }

    private void FixedUpdate()
    {
        rb.AddForce(playerVelocity * movementSpeed);
    }
}
