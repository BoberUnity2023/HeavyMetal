using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private GameObject _window;
    private bool _isPaused;    

    public void Update()
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
