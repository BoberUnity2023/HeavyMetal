using UnityEngine;

public class CarAI : MonoBehaviour, ICarInputable
{
    private WayPath _wayPath = null;
    private bool _isFinish = false;

    [SerializeField] private int currentWaypoint = 0;
    [SerializeField] private Vector3 _relativePointPosition;
    public int Laps = 1;
    private float _steer;
    private float _steerPrevious;

    private const float _steeringSpeed = 2;//Double by CarInput
    private const float _steeringBackSpeed = 4;//Double by CarInput

    private void Start()
    {
        if (IsAI)
            _wayPath = FindAnyObjectByType<WayPath>();
    }

    private void FixedUpdate()
    {
        if (IsAI)
        {
            _steerPrevious = _steer;
            Vector3 relativePointPosition = RelativePointPosition;
            _relativePointPosition = relativePointPosition;
            _steer = relativePointPosition.x / relativePointPosition.magnitude;

            if (relativePointPosition.magnitude < 15)
            {
                currentWaypoint++;
                //if (resultController != null && resultController.Results.Length > 0)
                //{
                //    resultController.Results[currentWaypoint + (Laps - 1) * waypoint.Waypoints.Length] += 1;
                //    resultController.CheckResults(); //
                //}
                if (currentWaypoint == _wayPath.Points.Length - 1)
                {
                    currentWaypoint = 0;
                    Laps++;
                }
            }
        }
    }

    public bool IsAI { get; set; }    

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

    private Vector3 RelativePointPosition
    {
        get
        {
            Transform point = _wayPath.Points[currentWaypoint].transform;
            Vector3 _pointPosition = new Vector3(point.position.x, transform.position.y, point.position.z);
            return transform.InverseTransformPoint(_pointPosition);
        }
    }
}
