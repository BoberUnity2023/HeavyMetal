using System;
using UnityEngine;

public class PathSelector : MonoBehaviour
{
    [SerializeField] private WayPath[] _wayPaths;

    private void Awake()
    {
        //WayPath[] paths = gameObject.GetComponentsInChildren<WayPath>();
        //Array.Resize(ref _wayPaths, paths.Length - 1);
    }

    public WayPath RandomWayPath
    {
        get
        {
            int rnd = UnityEngine.Random.Range(0, _wayPaths.Length);
            return _wayPaths[rnd];
        }
    }

    public WayPath WayPath(int id)
    {        
        return _wayPaths[id];
    }
}
