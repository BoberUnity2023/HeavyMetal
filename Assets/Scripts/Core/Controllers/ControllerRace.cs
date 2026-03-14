using System;
using UnityEngine;

public class ControllerRace : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    [SerializeField] private Car _carPrefab;
    [SerializeField] private Transform[] _carPositions;
    [SerializeField] private CameraMove _cameraMove;

    private Car _car;
    public Car Car => _car;   

    private void Awake()
    {
        for (int i = 0; i < _carPositions.Length; i++)
        {
            _car = Instantiate(_carPrefab, _carPositions[i].position, _carPositions[i].rotation);
            if (i < _carPositions.Length - 1)
            {                
                _car.Init(_hub, InputType.AI);
            }
            else
            {
                _car.Init(_hub, InputType.Player);
                _cameraMove.SetTarget(_car.transform);
            }
        }
    }
}
