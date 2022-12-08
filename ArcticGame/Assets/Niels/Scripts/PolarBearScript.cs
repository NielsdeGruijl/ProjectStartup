using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarBearScript : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bridgeCam;
    [SerializeField] private GameObject EndGameUI;
    [SerializeField] private PauseScript pause;

    private MountedMovement movement;

    private Animator anim;
    private Rigidbody rb;

    private GameObject bridgeCol;

    private float randWait;

    private string currentAnimState;

    private bool canIdle = true;
    private bool mounted = false;
    public bool moving = false;
    private bool nearBridge = false;
    private bool hasJumped = false;
    public bool isJumping = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        movement = GetComponent<MountedMovement>();
         
        StartCoroutine(Idle2());
    }

    private void Update()
    {
        mounted = player.activeInHierarchy;

        if (mounted)
            movement.enabled = true;
        else
            movement.enabled = false;

        moving = movement.GetVelocity() != Vector3.zero;

        SetAnimations();

        if (nearBridge)
            JumpBridge(bridgeCol);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Jump"))
        {
            nearBridge = true;
            bridgeCol = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Jump"))
        {
            nearBridge = false;
        }
    }

    private void SetAnimations()
    {
        if (isJumping)
            return;

/*        if (canIdle && !mounted)
        {
            randWait = Random.Range(3, 8);
            Debug.Log(randWait);
            StartCoroutine(Idle2());
        }*/
        
        if (mounted && !moving)
        {
            ChangeAnimationState("Standing");
        }
        if(mounted && moving)
        {
            ChangeAnimationState("Walking");
        }

        if (!mounted)
        {
            ChangeAnimationState("Idle1");
        }
    }
    
    private void JumpBridge(GameObject gObject)
    {
        if(!hasJumped)
        {
            ChangeAnimationState("Standing");
            isJumping = true;
            movement.canMove = false;
            bridgeCam.SetActive(true);
            transform.position = gObject.transform.position;
            transform.LookAt(gObject.transform.parent);
            StartCoroutine(WaitJump());
            hasJumped = true;
        }
        
        Debug.Log("Jumped the bridge");
    }

    public void ChangeAnimationState(string newAnimState)
    {
        if (currentAnimState != newAnimState)
            anim.Play(newAnimState);

        currentAnimState = newAnimState;
    }

    IEnumerator Idle2()
    {
        canIdle = false;
        yield return new WaitForSeconds(randWait);
        Debug.Log("Idle triggered");
        ChangeAnimationState("Idle2");
        canIdle = true;
    }

    IEnumerator WaitJump()
    {
        yield return new WaitForSeconds(2);
        movement.canMove = true;
        ChangeAnimationState("Jump");
        StartCoroutine(endGame());
    }

    IEnumerator endGame()
    {
        yield return new WaitForSeconds(3.5f);
        pause.gamePaused = true;
        EndGameUI.SetActive(true);
    }
}
