using TMPro;
using UnityEngine;

public class IndicatorPlace : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private TMP_Text _indicator;
    private int _countPlayers;

    private void Start()
    {
        _countPlayers = _hub.Level.Config.Enemies.Length + 1;
    }

    private void Update()
    {
        if (_hub.Level.Race.Car.IsFinished)
        {
            _indicator.text = "---";
            return;
        }
        
        _indicator.text = "" + _hub.Level.Race.Car.Place + "/" + _countPlayers.ToString();
    }
}
