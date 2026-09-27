using UnityEngine;

public class GarageAboutGame : WindowBase
{
    [SerializeField] private Garage _garage;
    [SerializeField] private string _url;

    public void PressClose()
    {
        _game.Sound.Play(SoundClip.Click);
        Hide();
        _garage.MainMenu.Show();
    }

    public void PressRate()
    {
        Application.OpenURL(_url);
    }
}
