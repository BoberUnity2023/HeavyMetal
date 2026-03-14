using System;
using UnityEngine;

public class WayPath : MonoBehaviour
{
    [SerializeField] private Color wayColor = new Color(1, 1, 1, 1);
    public Transform[] Points;
    public float[] MaxSpeeds;

    private void Awake()
    {
        int i = 0;
        var points = gameObject.GetComponentsInChildren<Transform>();
        Array.Resize(ref Points, points.Length - 1);
        Array.Resize(ref MaxSpeeds, points.Length - 1);
        foreach (Transform point in points)
        {
            if (point != transform)//чтобы не брал себя
            {
                Points[i] = point;
                MaxSpeeds[i] = point.GetComponent<WayPoint>().Speed / 2;
                i += 1;
            }
        }
    }

    void OnDrawGizmos()
    {
        var points = gameObject.GetComponentsInChildren<Transform>();
        if (!Application.isPlaying)
        {
            foreach (Transform point in points)
            {
                Gizmos.color = wayColor;
                Gizmos.DrawWireSphere(point.position, 10f);
            }
        }

        for (int i = 0; i < points.Length; i++)
        {
            if (i > 0)
                Gizmos.DrawLine(points[i].position, points[Mathf.Min(i + 1, points.Length - 1)].position);
        }
    }
}

