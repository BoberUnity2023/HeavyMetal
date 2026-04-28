using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum TuningType
{
    Engine,
    Shields,
    Tires,
    Weapons,
    Nitro
}

public class Bar : MonoBehaviour
{
    [SerializeField] private TuningType _type;
    [SerializeField] private Image _progress;
    [SerializeField] private TMP_Text _indicator;
    [SerializeField] private Button _buttonUpgrade;
    [SerializeField] private PriceIndicator _priceIndicator;
    private SceneController _sceneController;

    public void Init(SceneController sceneController)
    {
        _sceneController = sceneController;              
    }

    public void Set(float value, int price)
    {        
        _progress.fillAmount = value;
        _indicator.text = (value * 100).ToString("f0") + "%";
        
        bool _canUpgrade = CanUpgrade;
        _buttonUpgrade.gameObject.SetActive(_canUpgrade);
        _priceIndicator.gameObject.SetActive(_canUpgrade);
        _priceIndicator.SetPrice(price);
    }

    private bool CanUpgrade
    {
        get
        {
            GameController _game = _sceneController.Game;
            bool hasCar = _game.Saves.HasBoughtCar(_game.SelectedCarType);
            if (!hasCar)
                return false;

            bool isMax = false;
            ConfigCar _configCar = _game.ConfigGame.Cars[_game.SelectedCar];
            if (_type == TuningType.Engine)
                isMax = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Engine) == _configCar.Tuning.Engine.CountMax;

            if (_type == TuningType.Shields)
                isMax = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Shields) == _configCar.Tuning.Shields.CountMax;

            if (_type == TuningType.Tires)
                isMax = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Tires) == _configCar.Tuning.Tires.CountMax;

            if (_type == TuningType.Weapons)
                isMax = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Weapons) == _configCar.Tuning.Weapon.CountMax;

            if (_type == TuningType.Nitro)
                isMax = _game.Saves.GetTuning(_game.SelectedCarType, TuningType.Nitro) == _configCar.Tuning.Nitro.CountMax;

            return !isMax;
        }
    }
}
