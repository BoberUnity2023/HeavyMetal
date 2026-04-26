using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public enum TuningType
{
    Engine,
    Tires,
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
        GameController game = _sceneController.Game;
        Set(game.Saves.GetTuningEngine(game.SelectedCarType)/5);
    }

    public void Set(float value)
    {
        _progress.fillAmount = value;
        _indicator.text = (value * 100).ToString("f0") + "%";
        
        bool _canUpgrade = CanUpgrade;
        _buttonUpgrade.gameObject.SetActive(_canUpgrade);
        _priceIndicator.gameObject.SetActive(_canUpgrade);
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
            if (_type == TuningType.Engine)
                isMax = _game.Saves.GetTuningEngine(_game.SelectedCarType) == 5;
            if (_type == TuningType.Tires)
                isMax = _game.Saves.GetTuningTires(_game.SelectedCarType) == 5;
            if (_type == TuningType.Nitro)
                isMax = _game.Saves.GetTuningNitro(_game.SelectedCarType) == 5;

            return !isMax;
        }
    }
}
