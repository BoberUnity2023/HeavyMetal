using TMPro;
using UnityEngine;

public class IndicatorPlace : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private TMP_Text _indicator;
    [SerializeField] private TMP_Text _indicatorAll;    
    private int _lastDisplayedPlace = -1;
    private bool _hasFinishedDisplayed = false;

    private void Start()
    {
        int countPlayers = _hub.Level.Config.Enemies.Length + 1;
        _indicatorAll.text = countPlayers.ToString();
    }

    private void Update()
    {
        if (_hub.Level.Race.Car.IsFinished)
        {
            if (!_hasFinishedDisplayed)
            {
                _indicator.text = "";
                _hasFinishedDisplayed = true;
            }
            return;
        }

        int currentPlace = _hub.Level.Race.Car.Place;

        if (currentPlace != _lastDisplayedPlace)
        {
            _indicator.text = _hub.Level.Race.Car.Place.ToString(); ;
            
            _lastDisplayedPlace = currentPlace;
        }        
    }
}
