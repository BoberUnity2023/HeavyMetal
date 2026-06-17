using UnityEngine;

public class CarPositions : MonoBehaviour
{
    [SerializeField] private Transform[] _carPositions;

    public Transform Position(int number)
    { 
        return _carPositions[number]; 
    }

    public int Count => _carPositions.Length;

    void OnDrawGizmos()
    {
        var points = gameObject.GetComponentsInChildren<Transform>();
        if (!Application.isPlaying)
        {
            foreach (Transform point in points)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(point.position, 0.5f);
            }
        }

        for (int i = 0; i < points.Length; i++)
        {
            if (i > 0)
                Gizmos.DrawLine(points[i].position, points[Mathf.Min(i + 1, points.Length - 1)].position);
        }
    }
}
