using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private GameObject pausedMenu;

    public bool gamePaused = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gamePaused = !gamePaused;
            pausedMenu.SetActive(gamePaused);
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
