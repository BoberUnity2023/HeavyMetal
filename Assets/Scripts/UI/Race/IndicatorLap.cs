using UnityEngine;
using UnityEngine.UI;

public class IndicatorLap : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Text _indicator;

    private void Update()
    {
        _indicator.text = "Lap: " + _hub.Level.Race.Car.LapsCounter.Lap;
    }
}
