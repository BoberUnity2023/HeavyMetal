using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    private Vector3 _previousPosition;

    public Vector3 Force { get; private set; } 
    
}
