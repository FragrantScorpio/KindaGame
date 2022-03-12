using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    
    public void LowSettings()
    {
        QualitySettings.SetQualityLevel(0, true);
        print("bruh");
    }
    public void MiddleSettings()
    {
        QualitySettings.SetQualityLevel(1, true);
    }
    public void HighSettings()
    {
        QualitySettings.SetQualityLevel(2, true);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void ExitGame()
    {
        Application.Quit();
    }

}
