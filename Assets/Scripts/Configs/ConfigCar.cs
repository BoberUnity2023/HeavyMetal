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
    [SerializeField] private int _damageSpeed;
    [SerializeField] private int _damageImpulse;
    [SerializeField] private CarTuning _tuning;
    
    public CarType CarType => _carType;

    public Car Prefab => _prefab;

    public int Price => _price;

    public int MotorTorque => _motorTorque;

    public int BrakeTorque => _brakeTorque;

    public int MaxSpeed => _maxSpeed;

    public int DamageSpeed => _damageSpeed;

    public int DamageImpulse => _damageImpulse;

    public CarTuning Tuning => _tuning;
}
