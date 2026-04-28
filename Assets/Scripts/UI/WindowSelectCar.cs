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
    [SerializeField] private Bar _barShields;
    [SerializeField] private Bar _barTires;
    [SerializeField] private Bar _barWeapon;
    [SerializeField] private Bar _barNitro;
    private GameController _game;    

    private void Start()
    {
        _game = _sceneController.Game;
        _barEngine.Init(_sceneController);
        _barShields.Init(_sceneController);
        _barTires.Init(_sceneController);
        _barWeapon.Init(_sceneController);
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
        TryBuyTuning(TuningType.Engine, price, max);
    }

    public void PressBuyTuningShields()
    {
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Shields.Price;
        int max = configCar.Tuning.Shields.CountMax;
        TryBuyTuning(TuningType.Shields, price, max);
    }

    public void PressBuyTuningWeapon()
    {
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Weapon.Price;
        int max = configCar.Tuning.Weapon.CountMax;
        TryBuyTuning(TuningType.Weapons, price, max);
    }

    public void PressBuyTuningTires()
    {
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Tires.Price;
        int max = configCar.Tuning.Tires.CountMax;
        TryBuyTuning(TuningType.Tires, price, max);
    }

    public void PressBuyTuningNitro()
    {
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Nitro.Price;
        int max = configCar.Tuning.Nitro.CountMax;
        TryBuyTuning(TuningType.Nitro, price, max);
    }

    public void TryBuyTuning(TuningType tuningType, int price, int max)
    {        
        int current = _game.Saves.GetTuning(_game.SelectedCarType, tuningType);
        if (current < max && _game.Saves.Coins >= price)
        {
            _game.Coins -= price;
            _game.Saves.SetTuning(_game.SelectedCarType, tuningType, current + 1);
            _garage.CurrentCar.Tuning.SetTuning();
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
        int current = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Engine);
        int max = configCar.Tuning.Engine.CountMax;        
        float value = (float)current / max;
        int price = configCar.Tuning.Engine.Price;
        _barEngine.Set(value, price);

        current = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Shields);
        max = configCar.Tuning.Shields.CountMax;        
        value = (float)current / max;
        price = configCar.Tuning.Shields.Price;
        _barShields.Set(value, price);

        current = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Tires);
        max = configCar.Tuning.Tires.CountMax;        
        value = (float)current / max;
        price = configCar.Tuning.Tires.Price;
        _barTires.Set(value, price);

        current = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Weapons);
        max = configCar.Tuning.Weapon.CountMax;        
        value = (float)current / max;
        price = configCar.Tuning.Weapon.Price;
        _barWeapon.Set(value, price);

        current = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Nitro);
        max = configCar.Tuning.Nitro.CountMax;        
        value = (float)current / max;
        price = configCar.Tuning.Nitro.Price;
        _barNitro.Set(value, price);
    }
}
