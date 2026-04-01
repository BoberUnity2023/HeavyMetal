using UnityEngine;

public class LevelPaths : MonoBehaviour
{
    [SerializeField] private WayPath[] _wayPaths;
    public WayPath[] WayPaths => _wayPaths;
}
