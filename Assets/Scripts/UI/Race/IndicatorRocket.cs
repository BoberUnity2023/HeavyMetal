using UnityEngine;
using UnityEngine.UI;

public class IndicatorRocket : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    //[SerializeField] private Text _indicator;
    [SerializeField] private GameObject[] _rockets;

    private void Update()
    {
        int count = _hub.Level.Race.Car.RocketGun.Armo;
        //_indicator.text = "Rocket: " + count.ToString() + "/4";

        for (int i = 0; i < _rockets.Length; i++)
        {
            _rockets[i].SetActive(count > i);
        }
    }
}
