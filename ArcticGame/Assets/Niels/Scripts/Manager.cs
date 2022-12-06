using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mountedPlayer;
    [SerializeField] private GameObject mounterPolar;
    [SerializeField] private GameObject polarBear;
    [SerializeField] private CinemachineVirtualCamera vcam;

    private PlayerInteractions playerScript;
    private PlayerInteractions mountedPlayerScript;

    private bool hasMounted = false;

    private void Awake()
    {
        AudioManager.manager.PlayAudio("BGM");
    }

    private void Start()
    {
        playerScript = player.GetComponent<PlayerInteractions>();
        mountedPlayerScript = mountedPlayer.GetComponent<PlayerInteractions>();
    }

    private void Update()
    {
        if(playerScript.mounted && !hasMounted)
        {
            MountPlayer();
            hasMounted = true;
            mountedPlayerScript.mounted = true;
        }
        if(!mountedPlayerScript.mounted && hasMounted)
        {
            dismountPlayer();
            hasMounted = false;
            playerScript.mounted = false;
        }
    }

    public void MountPlayer()
    {
        player.SetActive(false);
        polarBear.SetActive(false);
        mountedPlayer.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z);
        mountedPlayer.transform.rotation = polarBear.transform.rotation;
        vcam.Follow = mountedPlayer.transform;
        mounterPolar.SetActive(true);
        mountedPlayer.SetActive(true);
    }

    public void dismountPlayer()
    {
        Debug.Log("dismounting player...");
        player.transform.position = mountedPlayer.transform.position;
        polarBear.transform.position = new Vector3(mountedPlayer.transform.position.x + 1, mountedPlayer.transform.position.y - 2.5f, mountedPlayer.transform.position.z);
        vcam.Follow = player.transform;
        player.SetActive(true);
        polarBear.SetActive(true);
        mountedPlayer.SetActive(false);
        mounterPolar.SetActive(false);
        Debug.Log("player dismounted");
    }
}
