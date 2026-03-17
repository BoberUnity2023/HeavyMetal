using UnityEditor.Build;
using UnityEngine;

public class LapsCounter : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private int _lap = 1;
    [SerializeField] private int _currentPoint = 0;
    [SerializeField] private int _targetPoint = 0;
    private WayPath _wayPath;    
    [SerializeField] private Vector3 _relativePointPosition;
    private bool _completed;
    [SerializeField] private int _p = 0;   
    public int Lap => _lap;

    public int Points => _currentPoint + (Lap - 1) * _wayPath.Points.Length;

    public Vector3 RelativePointPosition => _relativePointPosition;

    public void SetWayPath(WayPath wayPath)
    {
        _wayPath = wayPath;
    }

    private void FixedUpdate()
    {
        FixedUpdate_CalculateRelativePointPosition();
        FixedUpdate_CheckPoint();
        _p = Points;
    }

    private void OnTriggerExit(Collider other)
    {        
        if (other.gameObject.name == "Finish" && _completed)
        {            
            _completed = false;
            _currentPoint = 0;            
            _lap++;

            //Debug.LogWarning("Lap: " + Lap);
            bool isRaceCompleted = _lap > _car.Hub.Level.Config.Laps;
            if (isRaceCompleted)
            {
                //_lap = _car.Hub.Level.Config.Laps;
                _car.IsFinished = true;
                
                if (!_car.IsAI)
                    _car.Hub.Level.Race.Finish();   
                
                Debug.LogWarning("Finished");
            }
        }
    }    

    private void FixedUpdate_CheckPoint()
    {
        float checkDistance = _car.IsAI ? 20 : 25;
        
        if (_relativePointPosition.magnitude < checkDistance)
        {            
            _currentPoint = Mathf.Min(_currentPoint + 1, _wayPath.PointsCount);
            
            //if (resultController != null && resultController.Results.Length > 0)
            //{
            //    resultController.Results[_currentPoint + (Laps - 1) * waypoint.Waypoints.Length] += 1;
            //    resultController.CheckResults(); //
            //}
            if (_currentPoint == _wayPath.Points.Length - 1)
            {                
                _completed = true;
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
