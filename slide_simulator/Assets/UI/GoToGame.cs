using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneSwitcher : MonoBehaviour
{
    // Method to be called when the button is clicked
    public void SwitchScene(string Main)
    {
        SceneManager.LoadScene(Main);
    }
}
