using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z - 10);
    }
}
