using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuController : MonoBehaviour
{
    public void StartNewGame()
    {
        // Load your game's main level or scene
        SceneManager.LoadScene("YourGameScene");
    }

    public void OpenOptions()
    {
        // Implement code to open the options menu
    }

    public void QuitGame()
    {
        // Quit the application (only works in a standalone build)
        Application.Quit();
    }

}
