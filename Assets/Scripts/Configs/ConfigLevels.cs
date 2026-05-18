using UnityEngine;

[CreateAssetMenu(fileName = "ConfigLevels", menuName = "Configs/ConfigLevels")]
public class ConfigLevels : ScriptableObject
{
    [SerializeField] private ConfigLevel[] _levels;

    public ConfigLevel[] Levels => _levels;

    public ConfigLevel Level(int level)
    {
        return _levels[level - 1];
    }    
}
