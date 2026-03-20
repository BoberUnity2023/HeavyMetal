using UnityEngine;
using UnityEngine.UI;

public class IndicatorRocket : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Text _indicator;

    private void Update()
    {
        int count = _hub.Level.Race.Car.RocketGun.Armo;
        _indicator.text = "Rocket: " + count.ToString() + "/4";
    }
}
