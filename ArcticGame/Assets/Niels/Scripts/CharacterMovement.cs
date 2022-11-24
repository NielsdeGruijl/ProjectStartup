using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(Rigidbody))]
public class CharacterMovement : MonoBehaviour
{
    private GameObject animal;

    private CapsuleCollider playerCollider;
    private PlayableDirector director;
    private Rigidbody rb;
    private Camera cam;

    private Vector3 playerVelocity;

    public float movementSpeed;

    private float xVelocity;
    private float zVelocity;

    public bool mounted = false;
    private bool bobBefriended = false;
    private bool inAnimalRange = false;

    private void Awake()
    {
        if(rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }

        cam = Camera.main;
    }

    private void Start()
    {
        playerCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        if (!mounted)
        {
            Walking();
        }

        if (inAnimalRange)
        {
            AnimalInteraction();
        }

        if(Input.GetKeyUp(KeyCode.F) && mounted)
        {
            transform.SetParent(null);
            mounted = false;
            Debug.Log("You unmounted Bob");
        }

        Debug.Log(mounted);
        //CameraFollowPlayer();
    }

    private void FixedUpdate()
    {
        rb.AddForce(playerVelocity * movementSpeed);
    }

    private void Walking()
    {
        xVelocity = Input.GetAxisRaw("Horizontal");
        zVelocity = Input.GetAxisRaw("Vertical");

        playerVelocity = new Vector3(xVelocity, 0, zVelocity);
        playerVelocity = playerVelocity.normalized;

        transform.LookAt(new Vector3(transform.localPosition.x + playerVelocity.x, 1, transform.localPosition.z + playerVelocity.z));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PolarBear"))
        {
            Debug.Log("Press F to befriend Bob");
            animal = other.gameObject;
            inAnimalRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PolarBear"))
        {
            animal = null;
            inAnimalRange = false;
        }
    }

    void AnimalInteraction()
    {
        if(animal != null)
        {
            if (Input.GetKeyUp(KeyCode.F) && bobBefriended && !mounted)
            {
                Debug.Log("You mounted Bob");
                StartCoroutine(wait());
            }
            if (Input.GetKeyUp(KeyCode.F) && !bobBefriended && !mounted)
            {
                Debug.Log("You befriended Bob");
                bobBefriended = true;
            }
        }
    }

    private void MountAnimal()
    {
        //transform.SetParent(animal.transform);
        //director.Play();
        //transform.LookAt(mountedDirObject.transform);
        //transform.position = new Vector3(animal.transform.position.x, animal.transform.position.y + 1, animal.transform.position.z);
        //playerCollider.enabled = false;
        //rb.useGravity = false;
        StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("mounted set to True");
        mounted = true;
    }
}
