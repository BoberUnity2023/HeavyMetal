using UnityEngine;

public class WindowControl : WindowBase
{
    [SerializeField] private Garage _garage;
    [SerializeField] private Hub _hub;

    public void PressClose()
    {
        _game.Sound.Play(SoundClip.Click);
        Hide();
        if (_garage != null)
            _garage.MainMenu.Show();
        else
        {
            _hub.Game.UI.NavigationEnd();
            _hub.CanvasLevel.ControlsClose();
        }
    }
}
