using UnityEngine;

public class GamepadSelectLevel : MonoBehaviour
{
    [SerializeField] private WindowSelectLevel _windowSelectLevel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button1))
        {
            _windowSelectLevel.SelectLevelByGamepadNext();
            _windowSelectLevel.Game.UI.NavigationEnd();
        }

        if (Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            _windowSelectLevel.SelectLevelByGamepadPrev();
            _windowSelectLevel.Game.UI.NavigationEnd();
        }
    }
}
