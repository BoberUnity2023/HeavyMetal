using UnityEngine;

public class WindowControl : WindowBase
{
    [SerializeField] private Garage _garage;
    [SerializeField] private Hub _hub;

    public bool IsTutorialCompleted
    {
        get
        {
            return PlayerPrefs.GetInt("IsTutorialCompleted", 0) == 1;
        }

        set
        {
            PlayerPrefs.SetInt("IsTutorialCompleted", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }

    public void TryShowAsTutorial()
    {
        if (!IsTutorialCompleted)
        {
            Show();
        }
    }

    public void PressClose()
    {
        _game.Sound.Play(SoundClip.Click);
        Hide();
        
        if (_garage != null)
            _garage.MainMenu.Show();
        else
        {
            _hub.Game.UI.NavigationEnd();
            if (IsTutorialCompleted)
                _hub.CanvasLevel.ControlsClose();
            else
                _hub.RaceStarter.CloseTutorial();

            IsTutorialCompleted = true;
        }
    }
}
