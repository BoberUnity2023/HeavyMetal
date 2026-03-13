using UnityEngine;

[CreateAssetMenu(fileName = "Game", menuName = "Configs/ConfigGame")]
public class ConfigGame : ScriptableObject
{
    [SerializeField] private Platform _platform;

    public Platform Platform => _platform;
}
