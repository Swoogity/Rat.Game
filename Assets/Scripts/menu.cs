using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menu : MonoBehaviour
{
    public void OnPlayButton() 
    {
        SceneManager.LoadScene("Level_1");
    }

    public void OnLevelSelectButton() 
    {
        SceneManager.LoadScene("LevelSelect");
    }

    public void OnMenuButton() 
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnControlsButton()
    {
        SceneManager.LoadScene("Controls");
    }

    public void OnCreditsButton()
    {
        SceneManager.LoadScene("Credits");
    }

    public void OnQuitButton() 
    { 
        Application.Quit();
    }
}
