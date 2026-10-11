using UnityEngine;

public class CarTuningPanel : MonoBehaviour
{
    [SerializeField] private WindowSelectCar _windowSelectCar;
    [SerializeField] private Bar _barEngine;
    [SerializeField] private Bar _barShields;
    [SerializeField] private Bar _barTires;
    [SerializeField] private Bar _barWeapon;
    [SerializeField] private Bar _barNitro;
    [SerializeField] private Bar _barMines;
    [SerializeField] private ColorPanel _colorPanel;

    private SceneController _sceneController;
    private Garage _garage;
    private GameController _game;

    public void Init(SceneController sceneController, Garage garage)
    {
        _sceneController = sceneController;
        _game = sceneController.Game;
        _garage = garage;
        InitBars();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        SetBars();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void InitBars()
    {
        _barEngine.Init(_sceneController);
        _barShields.Init(_sceneController);
        _barTires.Init(_sceneController);
        _barWeapon.Init(_sceneController);
        _barNitro.Init(_sceneController);
        _barMines.Init(_sceneController);
        _colorPanel.Init(_sceneController, _garage);
    }

    private void SetBars()
    {
        ConfigCar configCar = _game.ConfigGame.Cars[_game.SelectedCar];
        SetBar(configCar.Tuning.Engine, TuningType.Engine, _barEngine);
        SetBar(configCar.Tuning.Shields, TuningType.Shields, _barShields);
        SetBar(configCar.Tuning.Tires, TuningType.Tires, _barTires);
        SetBar(configCar.Tuning.Weapon, TuningType.Weapons, _barWeapon);
        SetBar(configCar.Tuning.Nitro, TuningType.Nitro, _barNitro);
        SetBar(configCar.Tuning.Mines, TuningType.Mines, _barMines);
    }

    private void SetBar(TuningCategory category, TuningType tuningType, Bar bar)
    {
        int countTuning = _game.Saves.GetTuning(_game.SelectedCarType, tuningType);
        int max = category.CountMax;
        float value = (float)countTuning / max;        
        int price = countTuning < max ? category.Prices[countTuning] : 0;
        bar.Set(value, price);
    }
}
        