using UnityEngine;

public class WheelControl : MonoBehaviour
{
    [SerializeField] private Transform _wheelModel;
    [SerializeField] private Transform _wheelBody;
    [SerializeField] private Transform _wheelColliderHalf;
    private WheelCollider _wheelCollider;    
    private MeshCollider _modelMeshCollider;

    public Transform WheelModel => _wheelModel;

    public Transform WheelColliderHalf => _wheelColliderHalf;

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

        _wheelColliderHalf.transform.position = position;
        _wheelColliderHalf.localEulerAngles = Vector3.up * WheelCollider.steerAngle;
    }

    public void DamageRotationSet()
    {
        float rnd = Random.Range(0, 10);
        _wheelBody.localRotation = Quaternion.Euler(0, rnd, 0);
    }

    public void DamageRotationReset()
    {
        _wheelBody.localRotation = Quaternion.identity;
    }

    public bool IsAttached
    {
        get { return enabled; }        
    }
}
