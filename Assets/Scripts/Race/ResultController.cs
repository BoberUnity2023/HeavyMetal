using UnityEngine;

public class ResultController : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    public int FinishedEnemies { get; private set; }

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
        FinishedEnemies++;
    }
}
