using TMPro;
using UnityEngine;

public class IndicatorSpeed : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private TMP_Text _indicator;
    private int _lastDisplayedSpeed = -1;

    private void Update()
    {
        float speed = Mathf.Abs(_hub.Level.Race.Car.Speed * 3.6f);
        int roundedSpeed = Mathf.RoundToInt(speed);
        
        if (roundedSpeed != _lastDisplayedSpeed)
        {
            _indicator.text = roundedSpeed.ToString();
            _lastDisplayedSpeed = roundedSpeed;
        }        
    }
}
