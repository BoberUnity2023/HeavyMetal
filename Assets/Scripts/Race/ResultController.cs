using System.Collections.Generic;
using UnityEngine;

public class ResultController : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    private List<Car> _finishedCars = new List<Car>();

    public int FinishedEnemies => _finishedCars.Count;//{ get; private set; }

    public int Place => 1 + FinishedEnemies;

    public void StartRace()
    {
        foreach (Car enemy in _hub.Level.Race.Enemies)
        {
            enemy.LapsCounter.OnFinish += OnFinishCar;
        }
    }

    private void OnDestroy()
    {
        foreach (Car enemy in _hub.Level.Race.Enemies)
        {
            enemy.LapsCounter.OnFinish -= OnFinishCar;
        }
    }

    public void OnFinishCar(Car car)
    {
        if (_finishedCars.Contains(car))
            return;

        _finishedCars.Add(car);
    }
}
