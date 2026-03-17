using UnityEngine;
using UnityEngine.UI;

public class IndicatorPlace : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Text _indicator;

    private void Update()
    {
        int countPlayers = _hub.Level.Config.EnemyPrefabs.Length + 1;
        _indicator.text = "Place: " + _hub.Place.Place + "/" + countPlayers.ToString();
    }
}
