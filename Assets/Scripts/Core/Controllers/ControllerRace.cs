using System;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRace : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    [SerializeField] private Transform[] _carPositions;
    [SerializeField] private CameraMove _cameraMove;
    [SerializeField] private List<Car> _enemies;
    [SerializeField] private List<Car> _cars;

    private Car _car;
    public Car Car => _car;

    public bool IsStarted { get; private set; }

    public event Action<int> OnLapCompleted;
    public event Action OnFinish;    

    public void StartRace()//3-2-1 Completed
    {
        _hub.Result.StartRace();
        IsStarted = true;
    }

    public void Finish()
    {
        _hub.Level.Race.Car.IsFinished = true;
              
        _hub.Game.Saves.SaveLevelStars(StarsForLevel);

        AddFinishCoins();

        if (!_car.IsAI)
            OnFinish?.Invoke();
    }

    public List<Car> Enemies => _enemies;
    public List<Car> Cars => _cars;

    private void Awake()
    {
        _hub.Level.Init();
        _hub.PathSelector.Init();
        CreateCars();
    }

    public void LapCompleted(int lap)
    {
        OnLapCompleted?.Invoke(lap);
    }

    private void CreateCars()
    {
        for (int i = 0; i < _carPositions.Length; i++)
        {            
            if (i < _carPositions.Length - 1)            
                InitEnemy(i);            
            else            
                InitPlayer();            
        }
    }

    private void InitPlayer()
    {
        int id = _carPositions.Length - 1;
        Transform carPosition = _carPositions[id];        
        Car carPrefab = _hub.Game.ConfigGame.Cars[_hub.Game.SelectedCar].Prefab;
        Car car = Instantiate(carPrefab, carPosition.position, carPosition.rotation);
        _car = car;        
        car.Init(_hub, InputType.Player, 0, Mode.Track);
        _cameraMove.SetTarget(car.transform);
        _cars.Add(car);
    }

    private void InitEnemy(int id)
    {        
        Transform carPosition = _carPositions[id];
        ConfigEnemy configEnemy = _hub.Level.Config.Enemies[id];
        Car prefab = configEnemy.Car.Prefab;  
        Car car = Instantiate(prefab, carPosition.position, carPosition.rotation);
        car.Init(_hub, InputType.AI, id, Mode.Track);
        car.Tuning.SetEngine(configEnemy.TuningEngine);
        car.Tuning.SetShields(configEnemy.TuningShields);
        car.Tuning.SetTires(configEnemy.TuningTires);
        car.Tuning.SetWeapons(configEnemy.TuningWeapon);
        car.Tuning.SetNitro(configEnemy.TuningNitro);
        ConfigCar configCar = configEnemy.Car;
        Material material = configCar.Tuning.CarColors[configEnemy.TuningColor].Material;
        car.Paint.SetMaterial(material);

        _enemies.Add(car);
        _cars.Add(car);
        Debug.Log("Enemy " + prefab.gameObject.name + " created. Config: " + configEnemy.name);
    }

    private int StarsForLevel
    {
        get
        {
            int place = _hub.Result.Place;

            if (place == 1)
                return 3;
            if (place == 2)
                return 2;
            if (place == 3)
                return 1;

            return 0;
        }        
    }

    private void AddFinishCoins() 
    {
        int place = _hub.Result.Place;        
        if (place <= 3)
        {
            int prize = _hub.Level.Config.FinishCoins[place - 1];
            _hub.Game.Saves.Coins += prize;
        }       
    }
}
