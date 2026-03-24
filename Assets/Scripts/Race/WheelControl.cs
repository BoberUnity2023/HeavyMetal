using UnityEngine;

public class WheelControl : MonoBehaviour
{
    [SerializeField] private Transform _wheelModel;
    private WheelCollider _wheelCollider;    
    private MeshCollider _modelMeshCollider;

    public Transform WheelModel => _wheelModel;

    public WheelCollider WheelCollider => _wheelCollider;

    public MeshCollider ModelMeshCollider => _modelMeshCollider;

    public bool IsSteerable;
    public bool IsMotorized;
    
    private void Start()
    {
        _wheelCollider = GetComponent<WheelCollider>();
        _modelMeshCollider = _wheelModel.GetComponent<MeshCollider>();        
    }

    private void FixedUpdate()
    {        
        Vector3 position;
        Quaternion rotation;
        WheelCollider.GetWorldPose(out position, out rotation);
        _wheelModel.transform.position = position;
        _wheelModel.transform.rotation = rotation;
    } 

    public bool IsAttached
    {
        get { return enabled; }        
    }
}
