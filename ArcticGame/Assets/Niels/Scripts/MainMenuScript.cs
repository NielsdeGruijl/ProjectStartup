using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public void onPlayClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void onQuitClicked()
    {
        //Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

}
