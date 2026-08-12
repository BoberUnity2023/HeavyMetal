using UnityEngine;

public class PauseMenu : WindowBase
{
    [SerializeField] private Hub _hub;    
    private bool _isPaused;

    protected override void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            if (_isPaused)
                PauseOff();
            else
                PauseOn();
        }
    }

    private void PauseOn()
    {
        _window.SetActive(true);
        _isPaused = true;
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
    }

    private void PauseOff()
    {
        _window.SetActive(false);
        _isPaused = false;
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
    }

    public void OnPressContinue()
    {
        PauseOff();
    }

    public void OnPressRestart()
    {
        PauseOff();
        _hub.SceneLoader.LoadScene(_hub.Level.Config.SceneBuildIndex);
    }

    public void OnPressSettings()
    {
        Debug.Log("Press Settings");
    }

    public void OnPressGarage()
    {
        PauseOff();
        _hub.SceneLoader.LoadScene(1);
    }

    public void OnPressQuitGame()
    {
        Application.Quit();
    }
}
