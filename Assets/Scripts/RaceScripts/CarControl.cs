using UnityEngine;

public class CarControl: MonoBehaviour
{    
    [Header("Car Properties")]
    [SerializeField] private Car _car;
    //[SerializeField] private bool _ai;
    [SerializeField] private float motorTorque = 2000f;
    [SerializeField] private float brakeTorque = 2000f;
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float steeringRange = 30f;
    [SerializeField] private float steeringRangeAtMaxSpeed = 10f;
    [SerializeField] private float _downForce = 2000;
    [SerializeField] private float _speedForward;

    private Hub _hub;
    private WheelControl[] _wheels;
    private Rigidbody _rigidBody;
    private ICarInputable _carInput;

    //Calculate current speed along the car's forward axis
    public float Speed => Vector3.Dot(transform.forward, _rigidBody.linearVelocity); //O� -Max �� Max
    public float SpeedForward => _speedForward; //O� 0 �� Max        

    private void Start()
    {
        _hub = _car.Hub;
        _carInput = _car.AIInput.IsAI ? _car.AIInput : _hub.Input.CarInput;
        _rigidBody = GetComponent<Rigidbody>();

        // Get all wheel components attached to the car
        _wheels = GetComponentsInChildren<WheelControl>();
    }
    
    public void FixedUpdate()
    {
        // Get player input for acceleration and steering
        float force = _carInput.Force; // Forward/backward input
        float steering = _carInput.Steer; // Steering input        
        
        // Calculate current speed along the car's forward axis
        //float speed = Vector3.Dot(transform.forward, rigidBody.velocity);//linearVelocity
        
        // Normalized speed factor
        _speedForward = Mathf.InverseLerp(0, maxSpeed, Mathf.Abs(Speed)); ;

        // Reduce motor torque and steering at high speeds for better handling
        float currentMotorTorque = Mathf.Lerp(motorTorque, 0, SpeedForward);//��� SpeedForward == 0 - 1; ��� SpeedForward.Max == 0;
        float currentSteerRange = Mathf.Lerp(steeringRange, steeringRangeAtMaxSpeed, SpeedForward);

        foreach (WheelControl wheel in _wheels)
        {
            float _brakeTorque = 0;

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
                // Release brakes when accelerating
                wheel.WheelCollider.brakeTorque = 0f;
            }
            else
            {
                // Apply brakes when reversing direction
                wheel.WheelCollider.motorTorque = 0f;

                _brakeTorque = _carInput.Brake * brakeTorque; 
                
                if (!CanAccelerate)
                    _brakeTorque = brakeTorque;                               
            }
            
            if (!wheel.IsSteerable)
            {
                bool isAutoHandbrake = force == 0 && Mathf.Abs(Speed) < 1;
                float _handbrake = _carInput.Handbrake;
                
                if (isAutoHandbrake)
                    _handbrake = 1;

                _brakeTorque += _handbrake * brakeTorque;
            }
            
            wheel.WheelCollider.brakeTorque = _brakeTorque;
        }
        
        AddDownForce();
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
            Debug.Log(gameObject.name + ".Checkpoint");
            Checkpoint checkpoint = other.GetComponent<Checkpoint>();
            checkpoint.StartComplete();
        }
    }

    private void AddDownForce()
    {
        _rigidBody.AddForce(-transform.up * _downForce * SpeedForward);        
    }

    // Determine if the player is accelerating or trying to reverse
    public bool IsAccelerating => Mathf.Sign(_carInput.Force) == Mathf.Sign(Speed) || SpeedForward < 0.01f;

    private bool CanAccelerate
    {
        get
        {
            //if (_hub.Level.IsComplete)
            //    return false;

            //if (_hub.Level.IsLost)
            //    return false;

            return true;
        }
    }
}