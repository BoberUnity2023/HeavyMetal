using System;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRace : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    //[SerializeField] private Transform[] _carPositions;
    [SerializeField] private CameraMove _cameraMove;
    private List<Car> _enemies = new List<Car>();
    private List<Car> _cars = new List<Car>();

    private Car _car;
    public Car Car => _car;

    public bool IsStarted { get; private set; }
    public bool IsFinished => _hub.Level.Race.Car.IsFinished;

    public event Action<int> OnLapCompleted;
    public event Action OnFinish;    

    public void StartRace()//3-2-1 Completed
    {
        _hub.Result.StartRace();
        IsStarted = true;
        _hub.Game.Sound.Play(SoundClip.BattleBegin);
    }

    public void Finish()
    {
        _hub.Level.Race.Car.IsFinished = true;
              
        _hub.Game.Saves.SaveLevelStars(StarsForLevel);

        AddFinishCoins();

        if (_hub.Result.Place == 1)
            _hub.Game.Sound.Play(SoundClip.FirstPlace);

        if (!_car.IsAI)
            OnFinish?.Invoke();
    }

    public List<Car> Enemies => _enemies;
    public List<Car> Cars => _cars;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        _hub.Level.Init();
        InitWays();
        CreateCars();
    }

    public void LapCompleted(int lap)
    {
        OnLapCompleted?.Invoke(lap);
        if (lap == _hub.Level.Config.Laps - 1)
            _hub.Game.Sound.Play(SoundClip.LastLap);
    }

    private void CreateCars()
    {
        for (int i = 0; i < _hub.Level.CurrentLevelObjects.CarPositions.Count; i++)
        {
            if (i < _hub.Level.CurrentLevelObjects.CarPositions.Count - 1)
                InitEnemy(i);
            else
                InitPlayer();
        }
    }

    private void InitPlayer()
    {
        int id = _hub.Level.CurrentLevelObjects.CarPositions.Count - 1;
        Transform carPosition = _hub.Level.CurrentLevelObjects.CarPositions.Position(id);        
        Car carPrefab = _hub.Game.ConfigGame.Cars[_hub.Game.SelectedCar].Prefab;
        Car car = Instantiate(carPrefab, carPosition.position, carPosition.rotation);
        _car = car;        
        car.Init(_hub, InputType.Player, 0, Mode.Track);
        _cameraMove.SetTarget(car.transform);
        _cars.Add(car);
    }

    private void InitEnemy(int id)
    {        
        Transform carPosition = _hub.Level.CurrentLevelObjects.CarPositions.Position(id);
        ConfigEnemy configEnemy = _hub.Level.Config.Enemies[id];
        Car prefab = configEnemy.Car.Prefab;  
        Car car = Instantiate(prefab, carPosition.position, carPosition.rotation);
        car.Init(_hub, InputType.AI, id, Mode.Track);
        car.Tuning.SetEngine(configEnemy.TuningEngine);
        car.Tuning.SetShields(configEnemy.TuningShields);
        car.Tuning.SetTires(configEnemy.TuningTires);
        car.Tuning.SetWeapons(configEnemy.TuningWeapon);
        car.Tuning.SetNitro(configEnemy.TuningNitro);
        car.Tuning.SetMines(configEnemy.TuningMines);
        car.Tuning.SetShield(configEnemy.TuningShield);
        ConfigCar configCar = configEnemy.Car;
        Material material = configCar.Tuning.CarColors[configEnemy.TuningColor].Material;
        car.Paint.SetMaterial(material);

        _enemies.Add(car);
        _cars.Add(car);
        Debug.Log("Enemy " + prefab.gameObject.name + " created. Config: " + configEnemy.name);
    }

    private void InitWays()
    {
        foreach (WayPath wayPath in _hub.Level.CurrentLevelObjects.WayPaths)
        {
            wayPath.Init(_hub.Level.CurrentLevelObjects.Finish, _hub.Level.CurrentLevelObjects.Finish);
        }
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
