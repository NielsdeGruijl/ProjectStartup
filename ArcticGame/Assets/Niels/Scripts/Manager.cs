using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject mountedPlayer;
    [SerializeField] private GameObject polarBear;

    private CharacterMovement character;

    private bool hasMounted = false;

    private void Start()
    {
        character = player.GetComponent<CharacterMovement>();
    }

    private void Update()
    {
        if(character.mounted && !hasMounted)
        {
            MountPlayer();
            hasMounted = true;
        }
        if(!character.mounted && hasMounted)
        {
            demountPlayer();
            hasMounted = false;
        }
    }

    public void MountPlayer()
    {
        mountedPlayer.transform.position = player.transform.position;
        mountedPlayer.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.SetActive(false);
        polarBear.SetActive(false);
        mountedPlayer.SetActive(true);
    }

    public void demountPlayer()
    {
        player.transform.position = mountedPlayer.transform.position;
        polarBear.transform.position = new Vector3(mountedPlayer.transform.position.x + 1, mountedPlayer.transform.position.y, mountedPlayer.transform.position.z);
        player.SetActive(true);
        polarBear.SetActive(true);
        mountedPlayer.SetActive(false);
    }
}
