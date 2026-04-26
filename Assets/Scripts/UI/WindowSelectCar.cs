using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowSelectCar : WindowBase
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private PriceIndicator _priceIndicator;
    [SerializeField] private Button _buttonContinue;
    [SerializeField] private Button _buttonBuy;
    //[SerializeField] private Button _buttonTuning;
    [SerializeField] private Bar _barEngine;
    [SerializeField] private Bar _barTires;
    [SerializeField] private Bar _barNitro;
    private GameController _game;    

    private void Start()
    {
        _game = _sceneController.Game;
        _barEngine.Init(_sceneController);
        _barTires.Init(_sceneController);
        _barNitro.Init(_sceneController);        
        
        SetButtonsByCar(_game.SelectedCar);
    }

    public void PressNextCar()
    {
        _game.SelectedCar++;
        if (_game.SelectedCar == 4)
            _game.SelectedCar = 0;

        _garage.ShowCar(_game.SelectedCar);
        SetButtonsByCar(_game.SelectedCar);

        SetBars();
    }

    public void PressPreviousCar()
    {
        _game.SelectedCar--;
        if (_game.SelectedCar < 0)
            _game.SelectedCar = 3;

        _garage.ShowCar(_game.SelectedCar);
        SetButtonsByCar(_game.SelectedCar);

        SetBars();
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
        
    }

    private void SetButtonsByCar(int carId)
    {
        bool hasCar = _game.Saves.HasBoughtCar(_game.SelectedCarType);
        _buttonContinue.gameObject.SetActive(hasCar);
        //_buttonTuning.gameObject.SetActive(hasCar);
        _buttonBuy.gameObject.SetActive(!hasCar);
        _priceIndicator.gameObject.SetActive(!hasCar);
        int price = Price(_game.SelectedCar);
        _priceIndicator.SetPrice(price);
    }

    int Price(int carId)
    {
        return _game.CarPropses[_game.SelectedCar].Price;
    }

    private void SetBars()
    {
        _barEngine.Set(_game.Saves.GetTuningEngine(_game.SelectedCarType));
        _barTires.Set(_game.Saves.GetTuningTires(_game.SelectedCarType));
        _barNitro.Set(_game.Saves.GetTuningNitro(_game.SelectedCarType));
    }
}
