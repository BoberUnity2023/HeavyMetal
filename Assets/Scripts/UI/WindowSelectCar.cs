using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowSelectCar : WindowBase
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private PriceIndicator _priceIndicator;
    [SerializeField] private Button _buttonContinue;
    [SerializeField] private Button _buttonBuy;
    [SerializeField] private Button _buttonTuning;    
    private GameController _game;

    private void Start()
    {
        _game = _sceneController.Game;        
        SetButtonsByCar(_game.SelectedCar);
    }

    public void PressNextCar()
    {
        _game.SelectedCar++;
        if (_game.SelectedCar == 4)
            _game.SelectedCar = 0;

        _garage.ShowCar(_game.SelectedCar);
        SetButtonsByCar(_game.SelectedCar);
    }

    public void PressPreviousCar()
    {
        _game.SelectedCar--;
        if (_game.SelectedCar < 0)
            _game.SelectedCar = 3;

        _garage.ShowCar(_game.SelectedCar);
        SetButtonsByCar(_game.SelectedCar);
    }

    public void PressBuy()
    {
        bool hasCar = _game.Saves.HasBoughtCar(_game.SelectedCarType);
        int price = Price(_game.SelectedCar);
        if (!hasCar && _game.Saves.Coins >= price)
        {
            _game.Saves.Coins -= price;
            _game.Saves.SetBoughtCar(_game.SelectedCarType);
            SetButtonsByCar(_game.SelectedCar);
        }
    }

    public void PressTuning()
    {
        hide();
        _garage.WindowTuning.Show();
    }

    private void SetButtonsByCar(int carId)
    {
        bool hasCar = _game.Saves.HasBoughtCar(_game.SelectedCarType);
        _buttonContinue.gameObject.SetActive(hasCar);
        _buttonTuning.gameObject.SetActive(hasCar);
        _buttonBuy.gameObject.SetActive(!hasCar);
        _priceIndicator.gameObject.SetActive(!hasCar);
        int price = Price(_game.SelectedCar);
        _priceIndicator.SetPrice(price);
    }

    int Price(int carId)
    {
        return _game.CarPropses[_game.SelectedCar].Price;
    }
}
