using UnityEngine;

[CreateAssetMenu(fileName = "Game", menuName = "Configs/ConfigGame")]
public class ConfigGame : ScriptableObject
{
    [SerializeField] private Platform _platform;
    [SerializeField] private int _startCoins;

    public Platform Platform => _platform;

    public int StartCoins => _startCoins;
}
