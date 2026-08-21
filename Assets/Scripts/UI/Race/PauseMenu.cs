using UnityEngine;

public class PauseMenu : WindowBase
{
    [SerializeField] private CanvasLevel _canvasLevel;
    private Hub _hub;
    private bool _isPaused;

    public override void Init(GameController game)
    {
        base.Init(game);
        _hub = _canvasLevel.Hub;
    }

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
        _hub.Game.UI.NavigationEnd();
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
        Hide();
        _hub.CanvasLevel.WindowSettings.Show();
        _hub.Game.UI.NavigationEnd();
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
