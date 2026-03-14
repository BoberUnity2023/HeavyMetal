using UnityEngine;

public class CarAI : MonoBehaviour, ICarInputable
{
    private WayPath _wayPath = null;
    private bool _isFinish = false;

    [SerializeField] private Car _car;
    [SerializeField] private int _currentPoint = 0;
    [SerializeField] private Vector3 _relativePointPosition;
    [SerializeField] private Vector3 _f;
    public int Laps = 1;
    private float _steer;
    private float _steerPrevious;
    private bool _isReversing;
    private const float _reverceTime = 1;

    private const float _steeringSpeed = 2;//Double by CarInput
    private const float _steeringBackSpeed = 4;//Double by CarInput

    private void Start()
    {
        if (_car.InputType == InputType.AI)
            _wayPath = FindAnyObjectByType<WayPath>();
        FixedUpdate();
    }

    private void FixedUpdate()
    {
        if (_car.InputType == InputType.AI)
        {
            FixedUpdate_CalculateRelativePointPosition();
            FixedUpdate_CalculateSteer();
            FixedUpdate_CheckPoint();
        }
    }

    private void FixedUpdate_CalculateRelativePointPosition()
    {        
        Transform point = _wayPath.Points[_currentPoint].transform;
        Vector3 _pointPosition = new Vector3(point.position.x, transform.position.y, point.position.z);
        _relativePointPosition = transform.InverseTransformPoint(_pointPosition);
    }

    private void FixedUpdate_CalculateSteer()
    {
        _steerPrevious = _steer;
        _steer = _relativePointPosition.x / _relativePointPosition.magnitude;
    }

    private void FixedUpdate_CheckPoint()
    {
        if (_relativePointPosition.magnitude < 15)
        {
            _currentPoint++;
            //if (resultController != null && resultController.Results.Length > 0)
            //{
            //    resultController.Results[_currentPoint + (Laps - 1) * waypoint.Waypoints.Length] += 1;
            //    resultController.CheckResults(); //
            //}
            if (_currentPoint == _wayPath.Points.Length - 1)
            {
                _currentPoint = 0;
                Laps++;
            }
        }
    }

    //public InputType InputType { get; set; }    

    public float Steer
    {
        get
        {
            if (_steer > 0)
            {
                if (_steer > _steerPrevious)                
                    _steer = _steerPrevious + _steeringSpeed * Time.fixedDeltaTime;
                else
                {
                    if (_steer < _steerPrevious)
                        _steer = _steerPrevious - _steeringBackSpeed * Time.fixedDeltaTime;
                }
            }

            if (_steer < 0)
            {
                if (_steer > _steerPrevious)
                    _steer = _steerPrevious + _steeringBackSpeed * Time.fixedDeltaTime;
                else
                {
                    if (_steer < _steerPrevious)
                        _steer = _steerPrevious - _steeringSpeed * Time.fixedDeltaTime;
                }
            }
            return _steer;
        }
    }

    public float Force
    {
        get
        {
            return Mathf.Abs(_steer) < 0.1f ? 0.30f : 0.15f;
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
            return 0f;
        }
    }
}
