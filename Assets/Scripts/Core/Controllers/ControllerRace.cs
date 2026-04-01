using System;
using System.Collections.Generic;
using UnityEngine;

public class ControllerRace : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    //[SerializeField] private Car _carPrefab;
    [SerializeField] private Transform[] _carPositions;
    [SerializeField] private CameraMove _cameraMove;
    [SerializeField] private List<Car> _enemies;
    [SerializeField] private List<Car> _cars;

    private Car _car;
    public Car Car => _car;

    public bool IsStarted { get; private set; }

    public event Action OnFinish;

    public void StartRace()//3-2-1 Completed
    {
        _hub.Result.StartRace();
        IsStarted = true;
    }

    public void Finish()
    {
        _hub.Level.Race.Car.IsFinished = true;
        if (!_car.IsAI)
            OnFinish?.Invoke();
    }

    public List<Car> Enemies => _enemies;
    public List<Car> Cars => _cars;

    private void Awake()
    {
        CreateCars();
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
        Car carPrefab = _hub.Game.CarPropses[_hub.Game.SelectedCar].Prefab;
        Car car = Instantiate(carPrefab, carPosition.position, carPosition.rotation);
        _car = car;
        car.Init(_hub, InputType.Player, 0);
        _cameraMove.SetTarget(car.transform);
        _cars.Add(car);
    }

    private void InitEnemy(int id)
    {        
        Transform carPosition = _carPositions[id];
        Car prefab = _hub.Level.Config.EnemyPrefabs[id];  
        Car car = Instantiate(prefab, carPosition.position, carPosition.rotation);
        car.Init(_hub, InputType.AI, id);
        _enemies.Add(car);
        _cars.Add(car);
        Debug.Log("Enemy " + prefab.gameObject.name + " created");
    }
}
