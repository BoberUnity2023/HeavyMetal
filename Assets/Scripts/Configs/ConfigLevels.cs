using System;
using UnityEngine;

[Serializable] public class LevelCake
{
    [SerializeField] private LevelType _type;  

    public LevelType Type => _type;    
}

[CreateAssetMenu(fileName = "ConfigLevels", menuName = "Configs/ConfigLevels")]
public class ConfigLevels : ScriptableObject
{
    [SerializeField] private int _pathIndicatorMaxLevel;
    [SerializeField] private LevelCake[] _levelCakes;
    [SerializeField] private ConfigLevel[] _levels;

    public int PathIndicatorMaxLevel => _pathIndicatorMaxLevel;
    public ConfigLevel[] Levels => _levels;

    public LevelCake[] LevelCakes => _levelCakes;

    public ConfigLevel Level(int level)
    {
        return _levels[level - 1];
    }    
}
