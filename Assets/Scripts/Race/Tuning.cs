using UnityEngine;

public class Tuning : MonoBehaviour
{
    [SerializeField] private GameObject[] _engines;
    [SerializeField] private GameObject[] _shields;
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private GameObject[] _tires;
    private Car _car;
    private GameController _game;

    public void Init(Car car, GameController game)
    {        
        _car = car;
        _game = game;
        CheckByConfig();
        SetTuning();
    }

    public void SetTuning()
    {
        int engine = _game.Saves.GetTuning(_car.CarType, TuningType.Engine);
        int shields = _game.Saves.GetTuning(_car.CarType, TuningType.Shields);
        int tires = _game.Saves.GetTuning(_car.CarType, TuningType.Tires);
        int weapons = _game.Saves.GetTuning(_car.CarType, TuningType.Weapons);
        int nitro = _game.Saves.GetTuning(_car.CarType, TuningType.Nitro);

        for (int i = 0; i < _engines.Length; i++)
        {
            _engines[i].SetActive(i == engine - 1);
        }

        for (int i = 0; i < _shields.Length; i++)
        {
            _shields[i].SetActive(i < shields);
        }
    }

    private void CheckByConfig()
    {
        string carType = _car.CarType.ToString();
        ConfigCar configCar = _game.ConfigGame.Car(_car.CarType);
        int engineMax = configCar.Tuning.Engine.CountMax;
        if (_engines.Length != engineMax)
            Debug.LogError("No Equal Settings Car! " + carType + " Engine: Prefab: " + _engines.Length + "/ Config: " + engineMax);

        int shieldsMax = configCar.Tuning.Shields.CountMax;
        if (_shields.Length != shieldsMax)
            Debug.LogError("No Equal Settings Car!:" + carType + " Shields: Prefab: " + _shields.Length + "/ Config: " + shieldsMax);

        int weaponsMax = configCar.Tuning.Weapon.CountMax;
        if (_weapons.Length != weaponsMax)
            Debug.LogError("No Equal Settings Car!" + carType + "  Weapon: Prefab: " + _weapons.Length + "/ Config: " + weaponsMax);

        int tiresMax = configCar.Tuning.Tires.CountMax;
        if (_tires.Length != tiresMax)
            Debug.LogError("No Equal Settings Car!" + carType + "  Tires: Prefab: " + _tires.Length + "/ Config: " + tiresMax);
    }
}
