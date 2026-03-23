using UnityEngine;
using UnityEngine.UI;

public class IndicatorSpeed : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Text _indicator;

    private void Update()
    {
        float speed = Mathf.Abs(_hub.Level.Race.Car.Speed * 3.6f);
        _indicator.text = "Speed: " + speed.ToString("F0");
    }
}
