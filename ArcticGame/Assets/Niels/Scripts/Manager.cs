using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mountedPlayer;
    [SerializeField] private GameObject polarBear;

    private PlayerInteractions character;
    private PlayerInteractions mountedCharacter;

    private bool hasMounted = false;

    private void Start()
    {
        character = player.GetComponent<PlayerInteractions>();
        mountedCharacter = mountedPlayer.GetComponent<PlayerInteractions>();
    }

    private void Update()
    {
        if(character.mounted && !hasMounted)
        {
            MountPlayer();
            hasMounted = true;
            mountedCharacter.mounted = true;
        }
        if(!mountedCharacter.mounted && hasMounted)
        {
            dismountPlayer();
            hasMounted = false;
            character.mounted = false;
        }
    }

    public void MountPlayer()
    {
        player.SetActive(false);
        polarBear.SetActive(false);
        mountedPlayer.transform.position = new Vector3(polarBear.transform.position.x, polarBear.transform.position.y + 2.5f, polarBear.transform.position.z);
        mountedPlayer.transform.rotation = polarBear.transform.rotation;
        mountedPlayer.SetActive(true);
    }

    public void dismountPlayer()
    {
        Debug.Log("dismounting player...");
        player.transform.position = mountedPlayer.transform.position;
        polarBear.transform.position = new Vector3(mountedPlayer.transform.position.x + 1, mountedPlayer.transform.position.y - 2.5f, mountedPlayer.transform.position.z);
        player.SetActive(true);
        polarBear.SetActive(true);
        mountedPlayer.SetActive(false);
        Debug.Log("player dismounted");
    }
}
