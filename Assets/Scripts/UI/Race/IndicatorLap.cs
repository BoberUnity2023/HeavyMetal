using TMPro;
using UnityEngine;

public class IndicatorLap : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private TMP_Text _indicator;

    private void Update()
    {
        int lap = _hub.Level.Race.Car.LapsCounter.Lap;
        int laps = _hub.Level.Config.Laps;
        _indicator.text = "" + Mathf.Min(lap, laps).ToString() + "/" + laps.ToString();
    }
}
