using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Configs/ConfigLevel")]
public class ConfigLevel : ScriptableObject
{    
    [SerializeField] private Sprite _icon;

    [SerializeField] private int _sceneBuildIndex;

    [SerializeField] private int _track;

    [SerializeField] private int _laps;

    [SerializeField] private int _starsForOpen;    

    [SerializeField] private ConfigEnemy[] _enemies;

    [SerializeField] private int[] _finishCoins;

    public Sprite Icon => _icon;

    public int SceneBuildIndex => _sceneBuildIndex;

    public int Track => _track; 

    public int Laps => _laps;

    public int StarsForOpen => _starsForOpen;    

    public ConfigEnemy[] Enemies => _enemies;

    public int[] FinishCoins => _finishCoins;
}
