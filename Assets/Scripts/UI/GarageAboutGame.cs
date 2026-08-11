using UnityEngine;

public class GarageAboutGame : WindowBase
{
    [SerializeField] private Garage _garage;

    public void PressClose()
    {
        Hide();
        _garage.MainMenu.Show();
    }
}
