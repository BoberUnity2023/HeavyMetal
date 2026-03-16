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
            _car = Instantiate(_carPrefab, _carPositions[i].position, _carPositions[i].rotation);
            if (i < _carPositions.Length - 1)            
                InitEnemy(_car);            
            else            
                InitPlayer(_car);            
        }
    }

    private void InitPlayer(Car car)
    {
        car.Init(_hub, InputType.Player);
        _cameraMove.SetTarget(_car.transform);
    }

    private void InitEnemy(Car car)
    {
        _car.Init(_hub, InputType.AI);
        _enemies.Add(_car);
    }
}
