using UnityEngine;

public class GamePause : MonoBehaviour
{
    private bool _isPaused;
    
    public GamePause()
    {

    }

    public void Update_CkeckInput()
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
        _isPaused = true;
        Time.timeScale = 0.0f;
        AudioListener.pause = true;
    }

    private void PauseOff()
    {
        _isPaused = false;
        Time.timeScale = 1.0f;
        AudioListener.pause = false;
    }
}
