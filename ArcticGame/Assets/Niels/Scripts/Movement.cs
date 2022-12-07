using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private PauseScript pause;

    private PlayerAnimation animator;
    private PlayerInteractions playerScript;

    private Rigidbody rb;

    private Vector3 playerVelocity;

    public float movementSpeed;

    private float xVelocity;
    private float zVelocity;

    private bool canStartFootStep = true;
    public bool isWalking = false;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        playerScript = GetComponent<PlayerInteractions>();
        animator = GetComponent<PlayerAnimation>();            
    }

    private void Start()
    {
        if(animator.enabled && playerScript.mounted)
        {
            animator.enabled = false;
        }
        else
        {
            animator.enabled = true;
        }
    }

    private void Update()
    {
        if (pause.gamePaused)
            playerVelocity = Vector3.zero;
        else
            Walking();

        if (rb.velocity.x != 0 || rb.velocity.z != 0)
        {
            if (!playerScript.mounted && !isWalking)
            {
                animator.SetBoolean("Walking", true);
                isWalking = true;
            }

            if(canStartFootStep)
                StartCoroutine(FootSteps());
        }
        else
        {
            if (!playerScript.mounted && isWalking)
            {
                animator.SetBoolean("Walking", false);
                isWalking = false;
            }  
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
        int footstep = Random.Range(1, 5);
        Debug.Log(footstep);
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
