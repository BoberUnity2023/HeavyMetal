using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowSelectCar : WindowBase
{
    [SerializeField] private SceneController _sceneController;
    [SerializeField] private ColorPanel _colorPanel;
    [SerializeField] private PriceIndicator _priceIndicator;
    [SerializeField] private Button _buttonContinue;
    [SerializeField] private Button _buttonBuy;    
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
        _colorPanel.Init(_sceneController, _garage);

        SetButtonsByCar();
    }

    public void PressNextCar()
    {
        _game.SelectedCar++;
        if (_game.SelectedCar == 4)
            _game.SelectedCar = 0;

        _garage.ShowCar(_game.SelectedCar);
        SetButtonsByCar();
        
    }

    public void PressPreviousCar()
    {
        _game.SelectedCar--;
        if (_game.SelectedCar < 0)
            _game.SelectedCar = 3;

        _garage.ShowCar(_game.SelectedCar);
        SetButtonsByCar();        
    }

    public void PressBuy()
    {
        bool hasCar = _game.Saves.HasBoughtCar(_game.SelectedCarType);
        int price = Price();
        if (!hasCar && _game.Saves.Coins >= price)
        {
            _game.Coins -= price;
            _game.Saves.SetBoughtCar(_game.SelectedCarType);
            SetButtonsByCar();            
        }
    }

    public void PressBuyTuningEngine()
    {
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Engine.Price;
        int max = configCar.Tuning.Engine.CountMax;
        int current = _game.Saves.GetTuningEngine(_game.SelectedCarType);
        if (current < max && _game.Saves.Coins >= price)
        {
            _game.Coins -= price;
            _game.Saves.SetTuningEngine(_game.SelectedCarType, current + 1);
            SetButtonsByCar();
        }
    }

    public void PressBuyTuningTires()
    {
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Tires.Price;
        int max = configCar.Tuning.Tires.CountMax;
        int current = _game.Saves.GetTuningTires(_game.SelectedCarType);
        if (current < max && _game.Saves.Coins >= price)
        {
            _game.Coins -= price;
            _game.Saves.SetTuningTires(_game.SelectedCarType, current + 1);
            SetButtonsByCar();
        }
    }

    public void PressBuyTuningNitro()
    {
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Nitro.Price;
        int max = configCar.Tuning.Nitro.CountMax;
        int current = _game.Saves.GetTuningNitro(_game.SelectedCarType);
        if (current < max && _game.Saves.Coins >= price)
        {
            _game.Coins -= price;
            _game.Saves.SetTuningNitro(_game.SelectedCarType, current + 1);
            SetButtonsByCar();
        }
    }

    private void SetButtonsByCar()
    {
        bool hasCar = _game.Saves.HasBoughtCar(_game.SelectedCarType);
        _buttonContinue.gameObject.SetActive(hasCar);        
        _buttonBuy.gameObject.SetActive(!hasCar);
        _priceIndicator.gameObject.SetActive(!hasCar);
        int price = Price();
        _priceIndicator.SetPrice(price);
        SetBars();
        
        if (hasCar)
            _colorPanel.Show();
        else
            _colorPanel.Hide();
    }

    private int Price()
    {
        return _game.ConfigGame.Cars[_game.SelectedCar].Price;
    }

    private void SetBars()
    {
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int current = _game.Saves.GetTuningEngine(_game.SelectedCarType);
        int max = configCar.Tuning.Engine.CountMax;
        int price = configCar.Tuning.Engine.Price;
        float value = (float)current / max;        
        _barEngine.Set(value, price);

        current = _game.Saves.GetTuningTires(_game.SelectedCarType);
        max = configCar.Tuning.Tires.CountMax;
        price = configCar.Tuning.Tires.Price;
        value = (float)current / max;
        _barTires.Set(value, price);

        current = _game.Saves.GetTuningNitro(_game.SelectedCarType);
        max = configCar.Tuning.Nitro.CountMax;
        price = configCar.Tuning.Nitro.Price;
        value = (float)current / max;
        _barNitro.Set(value, price);
    }
}
