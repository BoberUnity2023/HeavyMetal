using System;
using UnityEngine;

public class PathSelector : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private LevelPaths[] _levelPaths;    
    private int _level;

    private void Awake()
    {
        //WayPath[] paths = gameObject.GetComponentsInChildren<WayPath>();
        //Array.Resize(ref _wayPaths, paths.Length - 1);
        int track = _hub.Level.Config.Track - 1;
        _level = track;// _hub.Game.CurrentLevel - 1;
    }

    public WayPath RandomWayPath
    {
        get
        {
            int rnd = UnityEngine.Random.Range(0, _levelPaths[_level].WayPaths.Length);
            return _levelPaths[_level].WayPaths[rnd];
        }
    }

    public WayPath WayPath(int id)
    {
        
        return _levelPaths[_level].WayPaths[id];
    }
}
