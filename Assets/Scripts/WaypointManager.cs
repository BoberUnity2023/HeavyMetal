using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaypointManager : MonoBehaviour
{
    public static WaypointManager Instance { get; private set; }

    public List<Transform> areaWaypoints = new List<Transform>();
    public bool loopPath = true;
    public float areaRadius = 6.0f;

    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Transform GetFirstWaypoint()
    {
        if (areaWaypoints.Count > 0)
        {
            return areaWaypoints[0];
        }
        return null;
    }

    public Transform GetNextWaypoint(int currentIndex)
    {
        if (areaWaypoints.Count == 0)
        {
            return null;
        }

        int nextIndex = currentIndex + 1;
        if (nextIndex < areaWaypoints.Count)
        {
            return areaWaypoints[nextIndex];
        }
        else if (loopPath)
        {
            return areaWaypoints[0];
        }
        return null; // End of path
    }

    public Transform GetWaypoint(int index)
    {
        if (index >= 0 && index < areaWaypoints.Count)
        {
            return areaWaypoints[index];
        }
        return null;
    }

    public Vector3 GetRandomPositionInWaypoint(int waypointIndex)
    {
        if (waypointIndex >= 0 && waypointIndex < areaWaypoints.Count)
        {
            Vector2 randomCircle = Random.insideUnitCircle * areaRadius;
            return areaWaypoints[waypointIndex].position + new Vector3(randomCircle.x, 0f, randomCircle.y);
        }
        return Vector3.zero;
    }

    // For debugging in editor
    void OnDrawGizmos()
    {
        if (areaWaypoints.Count == 0) return;

        Gizmos.color = Color.green;

        // Draw waypoint areas
        for (int i = 0; i < areaWaypoints.Count; i++)
        {
            Transform area = areaWaypoints[i];
            if (area)
            {
                // Draw area circle
                Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
                DrawCircle(area.position, areaRadius, 32);

                // Draw center point
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(area.position, 0.3f);

                // Draw lines between waypoint centers
                if (i < areaWaypoints.Count - 1 && areaWaypoints[i + 1] != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(area.position, areaWaypoints[i + 1].position);
                }
                else if (loopPath && areaWaypoints[0] != null)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(area.position, areaWaypoints[0].position);
                }

                // Draw waypoint numbers
#if UNITY_EDITOR
                UnityEditor.Handles.Label(area.position + Vector3.up * (areaRadius + 1f), $"Area: {area.name}\nRadius: {areaRadius}");
#endif
            }
        }
    }

    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angle = 0f;
        float angleIncrement = 360f / segments;
        Vector3 prevPoint = center + new Vector3(Mathf.Cos(0) * radius, 0, Mathf.Sin(0) * radius);

        for (int i = 1; i <= segments; i++)
        {
            angle = i * angleIncrement * Mathf.Deg2Rad;
            Vector3 nextPoint = center + new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
            Gizmos.DrawLine(prevPoint, nextPoint);
            prevPoint = nextPoint;
        }
    }
}