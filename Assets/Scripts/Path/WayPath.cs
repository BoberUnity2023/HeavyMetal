using System;
using UnityEngine;

public class WayPath : MonoBehaviour
{
    [SerializeField] private Color wayColor = new Color(1, 1, 1, 1);
    public Transform[] Points;
    public float[] MaxSpeeds;
    private Transform _start;
    private Transform _finish;
    private float _length;

    public int PointsCount => Points.Length;

    public void Init(Transform start, Transform finish)
    {
        _start = start;
        _finish = finish;

        int i = 0;
        var points = gameObject.GetComponentsInChildren<Transform>();
        Array.Resize(ref Points, points.Length - 1);
        Array.Resize(ref MaxSpeeds, points.Length - 1);
        foreach (Transform point in points)
        {
            if (point != transform)//чтобы не брал себя
            {
                Points[i] = point;
                MaxSpeeds[i] = point.GetComponent<WayPoint>().Speed;
                i += 1;
            }
        }

        CalculateLength();
    }

    private void CalculateLength()
    {
        _length = Vector3.Distance(_start.position, Points[0].position);

        for (int i = 0; i < Points.Length - 1; i++)
        {
            _length += Vector3.Distance(Points[i].position, Points[i + 1].position);
        }

        _length += Vector3.Distance(Points[Points.Length - 1].position, _finish.position);
    }

    //private void Awake()
    //{
    //    int i = 0;
    //    var points = gameObject.GetComponentsInChildren<Transform>();
    //    Array.Resize(ref Points, points.Length - 1);
    //    Array.Resize(ref MaxSpeeds, points.Length - 1);
    //    foreach (Transform point in points)
    //    {
    //        if (point != transform)//чтобы не брал себя
    //        {
    //            Points[i] = point;
    //            MaxSpeeds[i] = point.GetComponent<WayPoint>().Speed / 2;
    //            i += 1;
    //        }
    //    }
    //}

    private void OnDrawGizmos()
    {
        var points = gameObject.GetComponentsInChildren<Transform>();
        if (!Application.isPlaying)
        {
            Gizmos.color = wayColor;
            foreach (Transform point in points)
            {
                bool isI = point == transform;
                if (!isI)                    
                    Gizmos.DrawWireSphere(point.position, 15);
            }
            Gizmos.DrawWireSphere(points[1].position, 25);
        }

        for (int i = 0; i < points.Length; i++)
        {
            if (i > 0)
                Gizmos.DrawLine(points[i].position, points[Mathf.Min(i + 1, points.Length - 1)].position);
        }
    }

    public float Progress(int currentPoint, Vector3 position)
    {
        float length;

        if (currentPoint == Points.Length - 1)
        {
            length = Vector3.Distance(position, _finish.position);
        }
        else
        {
            length = Vector3.Distance(position, Points[currentPoint].position);
            for (int i = currentPoint; i < Points.Length - 1; i++)
            {
                length += Vector3.Distance(Points[i].position, Points[i + 1].position);
            }
            length += Vector3.Distance(Points[Points.Length - 1].position, _finish.position);
        }

        return 100 - length / _length * 100;
    }
}

