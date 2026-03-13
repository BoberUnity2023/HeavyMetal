using System;
using UnityEngine;

public class ControllerRace : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    [SerializeField] private Car _carPrefab;
    [SerializeField] private Transform _carPosition;
    [SerializeField] private CameraMove _cameraMove;

    private Car _car;
    public Car Car => _car;   

    private void Awake()
    {
        _car = Instantiate(_carPrefab, _carPosition.position, _carPosition.rotation);
        _car.Initi(_hub);
        _cameraMove.SetTarget(_car.transform);
    }
}
