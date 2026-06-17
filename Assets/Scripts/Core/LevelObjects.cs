using UnityEngine;

public class LevelObjects : MonoBehaviour
{    
    [SerializeField] private string _key;
    [SerializeField] private CarPositions _carPositions;
    [SerializeField] private Transform _finish;
    [SerializeField] private WayPath[] _wayPaths;

    public string Key => _key;

    public Transform Finish => _finish;

    public CarPositions CarPositions => _carPositions;

    public WayPath[] WayPaths => _wayPaths;

    public WayPath WayPath(int id)
    {
        return _wayPaths[id];
    }
}
