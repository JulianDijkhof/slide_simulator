using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    // Method to be called when the button is clicked
    public void GoToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Luuk");
    }
    public void GoToGame()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainScene");
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop playing in the Unity Editor
#else
            Application.Quit(); // Quit the application in a standalone build
#endif
    }
}

