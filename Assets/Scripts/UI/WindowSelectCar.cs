using UnityEngine;
using UnityEngine.UI;
using static Unity.VisualScripting.Dependencies.Sqlite.SQLite3;

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
            Game.Analitycs.SendBuyCar(_game.SelectedCarType);
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
        TryBuyTuning(configCar.Tuning.Engine, TuningType.Engine);
    }

    public void PressBuyTuningShields()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        TryBuyTuning(configCar.Tuning.Shields, TuningType.Shields);
    }

    public void PressBuyTuningWeapon()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        TryBuyTuning(configCar.Tuning.Weapon, TuningType.Weapons);
    }

    public void PressBuyTuningMines()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        TryBuyTuning(configCar.Tuning.Mines, TuningType.Mines);
    }

    public void PressBuyTuningTires()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        TryBuyTuning(configCar.Tuning.Tires, TuningType.Tires);
    }

    public void PressBuyTuningNitro()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        TryBuyTuning(configCar.Tuning.Nitro, TuningType.Nitro);        
    }

    public void PressBuyTuningShield()
    {
        Game.Sound.Play(SoundClip.Click);
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];           
        TryBuyTuning(configCar.Tuning.Shield, TuningType.Shield);
    }

    public void TryBuyTuning(TuningCategory category, TuningType tuningType)
    {        
        CarType carType = _game.SelectedCarType;
        int countTuning = _game.Saves.GetTuning(carType, tuningType);
        int price = category.Prices[countTuning];
        int max = category.CountMax;

        int current = _game.Saves.GetTuning(_game.SelectedCarType, tuningType);
        if (current < max && _game.Saves.Coins >= price)
        {
            _game.Coins -= price;
            _game.Sound.Play(SoundClip.Upgrade);
            _game.Saves.SetTuning(_game.SelectedCarType, tuningType, current + 1);
            _garage.CurrentCar.Tuning.SetTuning();
            SetButtonsByCar();
            _game.Analitycs.SendTuning(_garage.CurrentCar.CarType, tuningType, current + 1);
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
        SetBar(configCar.Tuning.Engine, TuningType.Engine, _barEngine);
        SetBar(configCar.Tuning.Shields, TuningType.Shields, _barShields);
        SetBar(configCar.Tuning.Tires, TuningType.Tires, _barTires);
        SetBar(configCar.Tuning.Weapon, TuningType.Weapons, _barWeapon);
        SetBar(configCar.Tuning.Mines, TuningType.Mines, _barMines);
        SetBar(configCar.Tuning.Nitro, TuningType.Nitro, _barNitro);
    }

    private void SetBar(TuningCategory category, TuningType tuningType, Bar bar)
    {
        int current = _game.Saves.GetTuning(_game.SelectedCarType, tuningType);
        int max = category.CountMax;
        float value = (float)current / max;
        int countTuning = _game.Saves.GetTuning(_game.SelectedCarType, tuningType);
        int price = category.Prices[countTuning];
        bar.Set(value, price);
    }

    private bool HasCar => _game.Saves.HasBoughtCar(_game.SelectedCarType);
}
