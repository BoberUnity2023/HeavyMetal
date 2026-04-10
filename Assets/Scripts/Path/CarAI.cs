using System.Collections;
using UnityEngine;

public class CarAI : MonoBehaviour, ICarInputable
{
    [SerializeField] private float _fullForceSpeed;

    private Car _car;
    private float _steer;
    private float _steerPrevious;

    private bool _isReversing;
    private const float _reverceTime = 0.75f;
    private float _currentCollapsTime = 0;

    private const float _steeringSpeed = 2;//Double by CarInput
    private const float _steeringBackSpeed = 4;//Double by CarInput

    public void Init(Car car)
    {
        _car = car;
    }

    public void FixedUpdate()
    {        
        if (_car.InputType == InputType.AI)
        {            
            FixedUpdate_CalculateSteer();
            FixedUpdate_TryReverse();
        }
    }   

    private void FixedUpdate_CalculateSteer()
    {
        _steerPrevious = _steer;
        _steer = _car.LapsCounter.RelativePointPosition.x / _car.LapsCounter.RelativePointPosition.magnitude;
    }

    private void FixedUpdate_TryReverse()
    {
        if (!_car.IsFinished &&
            _car.Input.Handbrake < 0.01f &&
            _car.Speed < 2 &&
            _car.Input.Force > 0.5f)
        {
            _currentCollapsTime += Time.fixedDeltaTime;
            if (_currentCollapsTime > 1)
            {
                AIReverseOn();
            }
        }
    }

    private void AIReverseOn()
    {
        _currentCollapsTime = 0;
        _isReversing = true;
        StartCoroutine(AIReverseOff(_reverceTime));
    }

    private IEnumerator AIReverseOff(float time)
    {
        yield return new WaitForSeconds(time);
        _isReversing = false;
        _currentCollapsTime = 0;
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

            if (_isReversing)
                return -1;

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

    public float Reverse
    {
        get
        {
            if (_isReversing)
                return 1;

            return 0f;
        }
    }
}
