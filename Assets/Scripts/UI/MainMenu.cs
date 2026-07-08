using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : WindowBase
{  
    public void OnPressNewGame()
    {
        Debug.Log("Press New Game");
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene(0);        
    }

    public void OnPressContinue()
    {
        Debug.Log("Press Continue");
        Hide();
        _garage.PressGame();
    }

    public void OnPressSettings()
    {
        Debug.Log("Press Settings");
        Hide();
        _garage.WindowSettings.Show();
    }

    public void OnPressQuitGame()
    {
        Debug.Log("Press Quit");
        Application.Quit();
    }
}
