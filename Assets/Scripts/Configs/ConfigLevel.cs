using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Configs/ConfigLevel")]
public class ConfigLevel : ScriptableObject
{    
    [SerializeField] private Sprite _icon;

    [SerializeField] private int _sceneBuildIndex;

    [SerializeField] private int _laps;

    [SerializeField] private int _starsForOpen;

    [SerializeField] private Car[] _enemyPrefabs;

    public Sprite Icon => _icon;

    public int SceneBuildIndex => _sceneBuildIndex;

    public int Laps => _laps;

    public int StarsForOpen => _starsForOpen;

    public Car[] EnemyPrefabs => _enemyPrefabs;
}
