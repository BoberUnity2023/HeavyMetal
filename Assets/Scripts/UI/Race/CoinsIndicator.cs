using TMPro;
using UnityEngine;

public class CoinsIndicator : MonoBehaviour
{
    [SerializeField] private SceneController _hub;
    [SerializeField] private TMP_Text _indicator;

    private void Start()
    {
        _hub.Game.Saves.OnCoinsChanged += OnCoinsChanged;        
        OnCoinsChanged(_hub.Game.Saves.Coins);
    }

    private void OnDestroy()
    {
        _hub.Game.Saves.OnCoinsChanged -= OnCoinsChanged;
    }

    private void OnCoinsChanged(int value)
    {
        _indicator.text = value.ToString();
    }
}
