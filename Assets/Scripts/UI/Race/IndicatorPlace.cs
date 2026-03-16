using UnityEngine;
using UnityEngine.UI;

public class IndicatorPlace : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Text _indicator;

    private void Update()
    {
        _indicator.text = "Place: " + _hub.Place.Place;
    }
}
