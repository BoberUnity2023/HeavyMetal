using UnityEngine;

public class Sun : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    private Vector3 _toCamera;

    void Start()
    {
        if (_hub == null)
            _hub = FindAnyObjectByType<Hub>();
        _toCamera = transform.position - _hub.Camera.transform.position;
    }
    
    void FixedUpdate()
    {
        transform.position = _hub.Camera.transform.position + _toCamera;
    }
}
