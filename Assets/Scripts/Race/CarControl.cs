using UnityEngine;

public class CarControl: MonoBehaviour
{    
    [Header("Car Properties")] 
    [SerializeField] private float _motorTorque = 2000f;
    [SerializeField] private float _brakeTorque = 2000f;
    [SerializeField] private float _maxSpeed = 20f;
    [SerializeField] private float _steeringRange = 30f;
    [SerializeField] private float _steeringRangeAtMaxSpeed = 10f;
    [SerializeField] private float _downForce = 2000;
    private float _angularDampingMultipler = 6.5f;    
    private Car _car;
    private float _angularDampingStart;
    //Calculate current speed along the car's forward axis
    public float MaxSpeed => _maxSpeed;

    public float EngineMultiplerDamage { get; set; }

    public float TuningTorque { get; set; }

    public void Init(Car car)
    {
        _car = car;
        Vector3 tensor = _car.Rigidbody.inertiaTensor;
        _car.Rigidbody.inertiaTensor = tensor * 2;
        _angularDampingStart = _car.Rigidbody.angularDamping;
        EngineMultiplerDamage = 1;
    }    

    public void FixedUpdate()
    {
        // Get player input for acceleration and steering        
        float force = _car.Input.Force; // Forward/backward input
        float steering = _car.Input.Steer; // Steering input 

        _car.Force = IsAccelerating && CanAccelerate ? force : 0;

        float torque = (_motorTorque + TuningTorque) * EngineMultiplerDamage;

        if (_car.Input.IsNitro)
            torque *= _car.Nitro.Multipler;

        float currentMotorTorque = _car.Input.IsNitro ? torque : Mathf.Lerp(torque, 0, _car.SpeedFactor);
        float currentSteerRange = Mathf.Lerp(_steeringRange, _steeringRangeAtMaxSpeed, _car.SpeedFactor);

        foreach (WheelControl wheel in _car.Wheels)
        {
            float brakeTorque = 0;

            // Apply steering to wheels that support steering
            if (wheel.IsSteerable)
            {
                wheel.WheelCollider.steerAngle = steering * currentSteerRange;
            }            

            if (IsAccelerating && CanAccelerate)
            {
                // Apply torque to IsMotorized wheels
                if (wheel.IsMotorized)
                {
                    wheel.WheelCollider.motorTorque = force * currentMotorTorque;
                }                
            }
            else
            {
                // Apply brakes when reversing direction
                wheel.WheelCollider.motorTorque = 0f;

                brakeTorque = _car.Input.Brake * _brakeTorque; 
                
                if (!CanAccelerate)
                    brakeTorque = _brakeTorque;                               
            }
            
            if (!wheel.IsSteerable)
            {
                bool isAutoHandbrake = force == 0 && Mathf.Abs(_car.Speed) < 1;
                float _handbrake = _car.Input.Handbrake;
                
                if (isAutoHandbrake || _car.IsFinished)
                    _handbrake = 1;

                brakeTorque += _handbrake * _brakeTorque;
            }
            
            wheel.WheelCollider.brakeTorque = brakeTorque;            
        }

        FixedUpdate_AddDownForce();
        //FixedUpdate_AddAngularDrag();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Car.OnTriggerEnter: " + other.gameObject.name);

        if (other.gameObject.tag == "FinishHero")
        {
            Debug.Log(gameObject.name + ".Finish");
            //_hub.Hero.Finish();
        }

        if (other.gameObject.tag == "Dead")
        {
            Debug.Log(gameObject.name + ".Dead");
            //_hub.Hero.Dead();
        }

        if (other.gameObject.name.Contains("Checkpoint"))
        {
            //Debug.Log(gameObject.name + ".Checkpoint");
            //Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            //checkpoint.StartComplete();
        }
    }

    private void FixedUpdate_AddDownForce()
    {
        _car.Rigidbody.AddForce(-transform.up * _downForce * _car.SpeedFactor);
        //if (_car.SlideForce > 0.25f)
        //    _car.Rigidbody.AddForce(-transform.up * _downForce * _car.SpeedFactor * (_car.SlideForce - 0.25f));
    }

    private void FixedUpdate_AddAngularDrag()
    {
        _car.Rigidbody.angularDamping = _angularDampingStart;

        if (_car.SlideForce > 0.5f)
            _car.Rigidbody.angularDamping = _angularDampingStart + (_car.SlideForce - 0.5f) * _angularDampingMultipler;
    }

    // Determine if the player is accelerating or trying to reverse
    public bool IsAccelerating => Mathf.Sign(_car.Input.Force) == Mathf.Sign(_car.Speed) || _car.SpeedFactor < 0.01f;

    public bool CanAccelerate
    {
        get
        {
            //if (_car.Hub.Level.IsComplete)
            //    return false;

            //if (_car.Hub.Level.IsLost)
            //    return false;

            if (!_car.Hub.Level.Race.IsStarted
                || _car.IsFinished
                || _car.IsCrashed
                )
                return false; 

            return true;
        }
    }
}