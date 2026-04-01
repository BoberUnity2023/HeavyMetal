using UnityEngine;

public class CarSelector : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private Garage _garage;    
    GameController _game;

    public void ClickNextCar()
    {
        _game = _sceneController.Game;

        _game.SelectedCar++;
        if (_game.SelectedCar == 4)
            _game.SelectedCar = 0;

        _garage.ShowCar(_game.SelectedCar);
    }

    public void ClickPreviousCar()
    {
        _game = _sceneController.Game;

        _game.SelectedCar--;
        if (_game.SelectedCar < 0)
            _game.SelectedCar = 3;

        _garage.ShowCar(_game.SelectedCar);
    }
}
