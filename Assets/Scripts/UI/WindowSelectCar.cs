using UnityEngine;
using UnityEngine.UI;

public class WindowSelectCar : WindowBase
{    
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private Garage _garage;
    [SerializeField] private ColorPanel _colorPanel;
    [SerializeField] private PriceIndicator _priceIndicator;
    [SerializeField] private Button _buttonContinue;
    [SerializeField] private Button _buttonBuy;    
    [SerializeField] private Bar _barEngine;
    [SerializeField] private Bar _barShields;
    [SerializeField] private Bar _barTires;
    [SerializeField] private Bar _barWeapon;
    [SerializeField] private Bar _barNitro;
    [SerializeField] private Bar _barMines;
    //[SerializeField] private Bar _barShield;
    [SerializeField] private GameObject _lock;    

    private void Awake()
    {
        _game = _sceneController.Game;
        _barEngine.Init(_sceneController);
        _barShields.Init(_sceneController);
        _barTires.Init(_sceneController);
        _barWeapon.Init(_sceneController);
        _barNitro.Init(_sceneController);
        _barMines.Init(_sceneController);
        _colorPanel.Init(_sceneController, _garage);
    }

    protected override void Start()
    {
        base.Start(); 

        SetButtonsByCar();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsActive)
            return;

        //Update_GamepadInput();
    }

    public override void Show()
    {
        base.Show();
        SetButtonsByCar();
    }

    protected override void SelectFirst(GameObject firstSelected)
    {
        Button button = HasCar ? _buttonContinue : _buttonBuy;
        base.SelectFirst(button.gameObject);
    }

    public void PressNextCar()
    {
        Game.Sound.Play(SoundClip.Click);
        _game.SelectedCar++;
        if (_game.SelectedCar == 3)
            _game.SelectedCar = 0;

        _garage.ShowCar(_game.SelectedCar);
        SetButtonsByCar();        
    }

    public void PressPreviousCar()
    {
        Game.Sound.Play(SoundClip.Click);
        _game.SelectedCar--;
        if (_game.SelectedCar < 0)
            _game.SelectedCar = 2;

        _garage.ShowCar(_game.SelectedCar);
        SetButtonsByCar();        
    }

    public void PressBuy()
    {
        Debug.Log("PressBuyCar()");
        Game.Sound.Play(SoundClip.Click);

        if (_game.SelectedCarType == CarType.Gnom)
            return;

        bool hasCar = _game.Saves.HasBoughtCar(_game.SelectedCarType);
        int price = Price();
        if (!hasCar && _game.Saves.Coins >= price)
        {
            _game.Coins -= price;
            _game.Sound.Play(SoundClip.Upgrade);
            _game.Saves.SetBoughtCar(_game.SelectedCarType);
            SetButtonsByCar();            
        }
    }

    public void PressBack()
    {
        Debug.Log("PressBack");
        Game.Sound.Play(SoundClip.Click);
        _garage.CameraMovier.MoveToCups();
        Hide();
        Invoke("MainMenuShow", 1);
    }

    private void MainMenuShow()
    {
        _garage.MainMenu.Show(); ;
    }

    public void PressBuyTuningEngine()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Engine.Price;
        int max = configCar.Tuning.Engine.CountMax;
        TryBuyTuning(TuningType.Engine, price, max);
    }

    public void PressBuyTuningShields()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Shields.Price;
        int max = configCar.Tuning.Shields.CountMax;
        TryBuyTuning(TuningType.Shields, price, max);
    }

    public void PressBuyTuningWeapon()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Weapon.Price;
        int max = configCar.Tuning.Weapon.CountMax;
        TryBuyTuning(TuningType.Weapons, price, max);
    }

    public void PressBuyTuningMines()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Mines.Price;
        int max = configCar.Tuning.Mines.CountMax;
        TryBuyTuning(TuningType.Mines, price, max);
    }

    public void PressBuyTuningTires()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Tires.Price;
        int max = configCar.Tuning.Tires.CountMax;
        TryBuyTuning(TuningType.Tires, price, max);
    }

    public void PressBuyTuningNitro()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Nitro.Price;
        int max = configCar.Tuning.Nitro.CountMax;
        TryBuyTuning(TuningType.Nitro, price, max);
    }

    public void PressBuyTuningShield()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        int price = configCar.Tuning.Shield.Price;
        int max = configCar.Tuning.Shield.CountMax;
        TryBuyTuning(TuningType.Shield, price, max);
    }

    public void TryBuyTuning(TuningType tuningType, int price, int max)
    {        
        int current = _game.Saves.GetTuning(_game.SelectedCarType, tuningType);
        if (current < max && _game.Saves.Coins >= price)
        {
            _game.Coins -= price;
            _game.Sound.Play(SoundClip.Upgrade);
            _game.Saves.SetTuning(_game.SelectedCarType, tuningType, current + 1);
            _garage.CurrentCar.Tuning.SetTuning();
            SetButtonsByCar();
        }
    }

    public void SetButtonsByCar()
    {
        bool hasCar = HasCar;
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

        _lock.SetActive(_game.SelectedCarType == CarType.Gnom && _game.ConfigGame.GameVersion == GameVersion.Demo);
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
        max = configCar.Tuning.Tires.CountMax - 1;        
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

        current = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Mines);
        max = configCar.Tuning.Mines.CountMax;
        value = (float)current / max;
        price = configCar.Tuning.Mines.Price;
        _barMines.Set(value, price);
    }

    private bool HasCar => _game.Saves.HasBoughtCar(_game.SelectedCarType);
}
