using System;
using UnityEngine;

[Serializable] public class TuningCategory
{
    [HideInInspector] public int CountBought;
    public int CountMax;
    public int Power;
    public int Price;
}

public class Tuning : MonoBehaviour
{
    [SerializeField] private TuningCategory _engine;
    [SerializeField] private TuningCategory _tires;
    [SerializeField] private TuningCategory _nitro;
    private Car _car;

    public TuningCategory Engine => _engine;
    public TuningCategory Tires => _tires;
    public TuningCategory Nitro => _nitro;
    
    public void Init(Car car)
    {
        _car = car;
        
        _engine.CountBought = _car.Hub.Game.Saves.GetTuningEngine(_car.CarType);
        _tires.CountBought = _car.Hub.Game.Saves.GetTuningTires(_car.CarType);
        _nitro.CountBought = _car.Hub.Game.Saves.GetTuningNitro(_car.CarType);
    }
}
