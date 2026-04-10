using System;
using UnityEditor.Build;
using UnityEngine;

public class LapsCounter : MonoBehaviour
{
    
    [SerializeField] private int _lap = 1;
    [SerializeField] private int _currentPoint;
    [SerializeField] private int _targetPoint;
    private WayPath _wayPath;
    private Car _car;
    private Vector3 _relativePointPosition;    
    private bool _isWayCompleted;    
    private bool _isRaceCompleted;
    
    public int Lap => _lap;

    public int Points 
    { 
        get 
        {
            return _currentPoint + (Lap - 1) * _wayPath.Points.Length; 
        } 
    }

    public int CurrentPoint => _currentPoint;

    public Vector3 RelativePointPosition => _relativePointPosition;    

    public event Action<int> OnLapStart;
    public event Action<Car> OnFinish;

    public void Init(Car car)
    {
        _car = car;
    }

    public void SetWayPath(WayPath wayPath)
    {
        _wayPath = wayPath;
    }

    private void FixedUpdate()
    {
        FixedUpdate_CalculateRelativePointPosition();
        FixedUpdate_CheckPoint();
        //_p = Points;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Finish" && _isWayCompleted)
        {
            if (_currentPoint < 10)//For back
                return;

            _isWayCompleted = false;
            
            _isRaceCompleted = _lap == _car.Hub.Level.Config.Laps;
            if (_isRaceCompleted)
            {
                //_lap = _car.Hub.Level.Config.Laps;
                _car.IsFinished = true;

                if (!_car.IsAI)
                    _car.Hub.Level.Race.Finish();

                OnFinish?.Invoke(_car);
                Debug.LogWarning("Finished");
            }
            else
            {   //Debug.LogWarning("Lap: " + Lap);
                _currentPoint = 0;
                _lap++;
                OnLapStart?.Invoke(_lap);
            }
        }
    }    

    private void FixedUpdate_CheckPoint()
    {
        float checkDistance = _car.IsAI ? 20 : 25;
        
        if (_relativePointPosition.magnitude < checkDistance)
        {            
            _currentPoint = Mathf.Min(_currentPoint + 1, _wayPath.PointsCount - 1);

            //if (resultController != null && resultController.Results.Length > 0)
            //{
            //    resultController.Results[_currentPoint + (Laps - 1) * waypoint.Waypoints.Length] += 1;
            //    resultController.CheckResults(); //
            //}

            bool isPointLast = _currentPoint == _wayPath.PointsCount - 1;
            if (isPointLast)
            {                
                _isWayCompleted = true;
                _targetPoint = 0;
            }
            else
            {                
                _targetPoint++;
            }
        }
    }

    private void FixedUpdate_CalculateRelativePointPosition()
    {
        Transform point = _wayPath.Points[_targetPoint].transform;
        Vector3 _pointPosition = new Vector3(point.position.x, transform.position.y, point.position.z);
        _relativePointPosition = transform.InverseTransformPoint(_pointPosition);
    }
}
