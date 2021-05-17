using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public void PlayGame()
    {
        SceneManager.LoadScene("Map SOLO");
        Time.timeScale = 1f;
        GameIsPaused = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void OpenMultiMenu()
    {
        SceneManager.LoadScene("MenuOfMulti");
    }

    public void QuitGame()
    {
        Debug.Log("QUIT !");
        Application.Quit();
    }
}
