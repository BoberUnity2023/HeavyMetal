using UnityEngine;

public class WheelControl : MonoBehaviour
{
    [SerializeField] private Transform _wheelModel;
    [HideInInspector] public WheelCollider WheelCollider;

    public bool IsSteerable;
    public bool IsMotorized;
    
    private void Start()
    {
        WheelCollider = GetComponent<WheelCollider>();
    }

    private void FixedUpdate()
    {        
        Vector3 position;
        Quaternion rotation;
        WheelCollider.GetWorldPose(out position, out rotation);
        _wheelModel.transform.position = position;
        _wheelModel.transform.rotation = rotation;
    } 
}
