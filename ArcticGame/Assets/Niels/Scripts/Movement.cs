using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private PauseScript pause;

    private PlayerAnimation animator;

    private Rigidbody rb;

    private Vector3 playerVelocity;

    public float movementSpeed;

    private float xVelocity;
    private float zVelocity;

    private bool canStartFootStep = true;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        animator = GetComponent<PlayerAnimation>();

        animator.ChangeAnimationState("Idle");
    }

    private void Update()
    {
        if (pause.gamePaused)
            playerVelocity = Vector3.zero;
        else
            Walking();

        if (rb.velocity.x != 0 || rb.velocity.z != 0)
        {
            animator.ChangeAnimationState("Walking");

            if(canStartFootStep)
                StartCoroutine(FootSteps());
        }
        else
        {
            animator.ChangeAnimationState("Idle");
        }
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

    private IEnumerator FootSteps()
    {
        canStartFootStep = false;
        AudioManager.manager.PlayAudio("FootStep1");
        yield return new WaitForSeconds(0.5f);
        canStartFootStep = true;
    }
}
