using UnityEngine;

public class Tuning : MonoBehaviour
{
    [SerializeField] private GameObject[] _engines;
    [SerializeField] private GameObject[] _shields;
    [SerializeField] private GameObject _defaultWeapon;
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private GameObject[] _defaultTires;
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

        SetEngine(engine);
        SetShields(shields);
        SetTires(tires);
        SetWeapons(weapons);
        SetNitro(nitro);
    }

    private void SetEngine(int engine)
    {
        for (int i = 0; i < _engines.Length; i++)
        {
            _engines[i].SetActive(i == engine - 1);            
        }

        if (_car.Mode == Mode.Track && !_car.IsAI)
        {
            ConfigCar configCar = _game.ConfigGame.Car(_car.CarType);
            int tuningTorque = engine * (int)configCar.Tuning.Engine.Power;
            _car.Control.TuningTorque = tuningTorque;            
        }
    }

    private void SetShields(int shields)
    {
        for (int i = 0; i < _shields.Length; i++)
        {
            _shields[i].SetActive(i < shields);
        }
    }

    private void SetTires(int tires)
    {
        if (_car.Mode == Mode.Track)
        {
            for (int i = 0; i < _car.WheelSkids.Length; i++)
            {
                _car.WheelSkids[i].TuningFactor = 1 + tires * 0.1f;
            }
        }      
    }

    private void SetWeapons(int weapons)
    {
        _defaultWeapon.SetActive(weapons == 0);

        for (int i = 0; i < _weapons.Length; i++)
        {
            _weapons[i].SetActive(i == weapons - 1);
        }

        if (_car.Mode == Mode.Track)
            _car.RocketGun.SetTuningWeapon(weapons);
    }

    private void SetNitro(int nitro)
    {
        ConfigCar configCar = _game.ConfigGame.Car(_car.CarType);
        float time = configCar.Tuning.Nitro.Power * nitro;
        _car.Nitro.AddTuningTime(time);
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
