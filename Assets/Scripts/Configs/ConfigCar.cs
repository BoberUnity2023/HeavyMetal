using UnityEngine;

[CreateAssetMenu(fileName = "Car", menuName = "Configs/ConfigCar")]


public class ConfigCar : ScriptableObject
{
    [SerializeField] private CarType _carType;
    [SerializeField] private Car _prefab;
    [SerializeField] private int _price;
    [SerializeField] private int _motorTorque;
    [SerializeField] private int _brakeTorque;
    [SerializeField] private int _maxSpeed;
    [SerializeField] private CarTuning _tuning;
    
    public CarType CarType => _carType;

    public Car Prefab => _prefab;

    public int Price => _price;

    public int MotorTorque => _motorTorque;

    public int BrakeTorque => _brakeTorque;

    public int MaxSpeed => _maxSpeed;

    public CarTuning Tuning => _tuning;
}
