using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class ControllerRace : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    [SerializeField] private Car _carPrefab;
    [SerializeField] private Transform[] _carPositions;
    [SerializeField] private CameraMove _cameraMove;
    [SerializeField] private List<Car> _enemies;

    private Car _car;
    public Car Car => _car;

    public List<Car> Enemies => _enemies;

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
        Car car = Instantiate(_carPrefab, carPosition.position, carPosition.rotation);
        _car = car;
        car.Init(_hub, InputType.Player, 0);
        _cameraMove.SetTarget(car.transform);
    }

    private void InitEnemy(int id)
    {        
        Transform carPosition = _carPositions[id];
        Car prefab = _hub.Level.Config.EnemyPrefabs[id];  
        Car car = Instantiate(prefab, carPosition.position, carPosition.rotation);
        car.Init(_hub, InputType.AI, id);
        _enemies.Add(car);
        Debug.Log("Enemy " + prefab.gameObject.name + " created");
    }
}
