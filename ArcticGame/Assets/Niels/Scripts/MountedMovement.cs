using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MountedMovement : MonoBehaviour
{
    [SerializeField] private PauseScript pause;

    private Rigidbody rb;

    private Vector3 playerVelocity;

    public float movementSpeed;

    private float xVelocity;
    private float zVelocity;

    private bool canStartFootStep = true;
    public bool canMove = true;
    public bool moving = false;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        if (pause.gamePaused || !canMove)
            playerVelocity = Vector3.zero;
        else
            Walking();

        if (rb.velocity.x != 0 || rb.velocity.z != 0)
        {
            moving = true;
            if (canStartFootStep)
                StartCoroutine(FootSteps());
        }
        else
        {
            moving = false;
        }

        //Debug.Log(playerVelocity);
    }

    private void Walking()
    {
        if(canMove)
        {
            xVelocity = Input.GetAxisRaw("Horizontal");
            zVelocity = Input.GetAxisRaw("Vertical");
        }

        playerVelocity = new Vector3(xVelocity, 0, zVelocity);
        playerVelocity = playerVelocity.normalized;

        transform.LookAt(new Vector3(transform.localPosition.x + playerVelocity.x, transform.position.y, transform.localPosition.z + playerVelocity.z));
    }

    private void FixedUpdate()
    {
        rb.AddForce(playerVelocity * movementSpeed);
    }

    public Vector3 GetVelocity()
    {
        return playerVelocity;
    }

    private IEnumerator FootSteps()
    {
        canStartFootStep = false;
        int footstep = Random.Range(1, 5);
        //Debug.Log(footstep);
        string audioFile = "";

        switch (footstep)
        {
            case 1:
                audioFile = "FootStep1";
                break;
            case 2:
                audioFile = "FootStep2";
                break;
            case 3:
                audioFile = "FootStep3";
                break;
            case 4:
                audioFile = "FootStep4";
                break;
        }

        AudioManager.manager.PlayAudio(audioFile);

        yield return new WaitForSeconds(0.5f);
        canStartFootStep = true;
    }
}

