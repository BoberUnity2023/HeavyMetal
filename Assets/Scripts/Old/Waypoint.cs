using UnityEngine;
using System;

public class Waypoint : MonoBehaviour
{
    [SerializeField] private Color wayColor = new Color(1, 1, 1, 1);
    private Transform[] waypoints;  
    
    public Transform[] Waipoints
    {
        get { return waypoints; }
    }

    private void Awake()
    {
        int i = 0;
        Transform[] _points = gameObject.GetComponentsInChildren<Transform>();
        Array.Resize(ref waypoints, _points.Length - 1);        
        foreach (Transform _point in _points)
        {
            if (_point != transform)//чтобы не брал себя
            {
                waypoints[i] = _point;                
                i += 1;
            }
        }
    }

    void OnDrawGizmos()
    {
        Transform[] _points = gameObject.GetComponentsInChildren<Transform>();
        if (!Application.isPlaying)
        {
            foreach (Transform _point in _points)
            {
                float r = _point != transform ? 0.15f : 0.1f;                
                Gizmos.color = wayColor;
                Gizmos.DrawSphere(_point.position, r);
            }
        }

        for (int i = 0; i < _points.Length; i++)
        {
            if (i > 0)
                Gizmos.DrawLine(_points[i].position, _points[Mathf.Min(i + 1, _points.Length - 1)].position);
        }
    }
}
