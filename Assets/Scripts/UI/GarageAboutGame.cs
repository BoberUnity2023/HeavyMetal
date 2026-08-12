using UnityEngine;

public class GarageAboutGame : WindowBase
{
    [SerializeField] private Garage _garage;

    public void PressClose()
    {
        Game.Sound.Play(SoundClip.Click);
        Hide();
        _garage.MainMenu.Show();
    }
}
