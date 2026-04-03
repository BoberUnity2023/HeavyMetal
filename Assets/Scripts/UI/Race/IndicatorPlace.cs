using TMPro;
using UnityEngine;

public class IndicatorPlace : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private TMP_Text _indicator;

    private void Update()
    {
        if (_hub.Level.Race.Car.IsFinished)
        {
            _indicator.text = "---";
            return;
        }

        int countPlayers = _hub.Level.Config.EnemyPrefabs.Length + 1;
        _indicator.text = "" + _hub.Place.Place + "/" + countPlayers.ToString();
    }
}
