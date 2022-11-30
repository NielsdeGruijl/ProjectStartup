using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pausedMenu;
    [SerializeField] private PlayerInteractions playerScript;

    public bool gamePaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            pausedMenu.SetActive(gamePaused);
        }
    }
}
