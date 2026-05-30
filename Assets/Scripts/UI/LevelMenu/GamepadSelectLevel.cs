using UnityEngine;

public class GamepadSelectLevel : MonoBehaviour
{
    [SerializeField] private MainMenuController _mainMenuController;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            _mainMenuController.SelectLevelByGamepadNext();
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            _mainMenuController.SelectLevelByGamepadPrev();
        }
    }
}
