using TMPro;
using UnityEngine;

public class IndicatorSpeed : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private TMP_Text _indicator;

    private void Update()
    {
        float speed = Mathf.Abs(_hub.Level.Race.Car.Speed * 3.6f);
        _indicator.text = "" + speed.ToString("F0");
    }
}
