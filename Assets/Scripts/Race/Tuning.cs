using UnityEngine;

public class Tuning : MonoBehaviour
{
    [SerializeField] private GameObject[] _engines;
    [SerializeField] private GameObject[] _shields;
    [SerializeField] private GameObject[] _weapons;
    private Car _car;
    
    public void Init(Car car)
    {
        _car = car;
        
        int engine = _car.Hub.Game.Saves.GetTuning(_car.CarType, TuningType.Engine);
        int shields = _car.Hub.Game.Saves.GetTuning(_car.CarType, TuningType.Shields);        
        int tires = _car.Hub.Game.Saves.GetTuning(_car.CarType,TuningType.Tires);
        int weapons = _car.Hub.Game.Saves.GetTuning(_car.CarType, TuningType.Weapons);
        int nitro = _car.Hub.Game.Saves.GetTuning(_car.CarType,TuningType.Nitro);
    }
}
