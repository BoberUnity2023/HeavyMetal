using UnityEditor.Recorder;
using UnityEngine;

public class CarAI : MonoBehaviour, ICarInputable
{
    [SerializeField] private Car _car;
    [SerializeField] private float _fullForceSpeed;

    [SerializeField] private float _s;

    private float _steer;
    private float _steerPrevious;
    private bool _isReversing;
    private const float _reverceTime = 1;

    private const float _steeringSpeed = 2;//Double by CarInput
    private const float _steeringBackSpeed = 4;//Double by CarInput

    private void Start()
    {
        
    }

    public void FixedUpdate()
    {
        _s = _car.Speed;
        if (_car.InputType == InputType.AI)
        {            
            FixedUpdate_CalculateSteer();            
        }
    }   

    private void FixedUpdate_CalculateSteer()
    {
        _steerPrevious = _steer;
        _steer = _car.LapsCounter.RelativePointPosition.x / _car.LapsCounter.RelativePointPosition.magnitude;
    }
    
    public float Steer
    {
        get
        {
            if (_car.IsFinished)
                return 1;

            float output = 0;

            if (_steer > 0)
            {
                if (_steer > _steerPrevious)
                    output = Mathf.Min(_steer, _steerPrevious + _steeringSpeed * Time.fixedDeltaTime);
                else
                {
                    if (_steer < _steerPrevious)
                        output = Mathf.Max(0, _steerPrevious - _steeringBackSpeed * Time.fixedDeltaTime);
                }
            }

            if (_steer < 0)
            {
                if (_steer > _steerPrevious)
                    output = Mathf.Min(0, _steerPrevious + _steeringBackSpeed * Time.fixedDeltaTime);
                else
                {
                    if (_steer < _steerPrevious)
                        output = Mathf.Max(_steerPrevious - _steeringSpeed * Time.fixedDeltaTime, _steer);
                }
            }
            return output;
        }
    }

    public float Force
    {
        get
        {            
            if (!_car.Control.CanAccelerate)
                return 0;
            
            if (_car.IsFinished)
                return 0;

            if (_car.Speed < _fullForceSpeed)
            {
                _car.Force = 1;
                return 1;
            }
            
            float output = Mathf.Abs(_steer) < 0.1f ? 0.25f : 0.10f;
            _car.Force = output;
            return output; 
        }
    }

    public float Brake
    {
        get
        {
            return 0f;
        }
    }

    public float Handbrake
    {
        get
        {
            if (_car.IsFinished)
                return 1;

            return 0f;
        }
    }
}
