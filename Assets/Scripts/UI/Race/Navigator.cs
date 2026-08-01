using UnityEngine;
using UnityEngine.UI;

public class Navigator : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private Image _back;
    [SerializeField] private float _showTime;
    private float _backTime;

    private void Start()
    {
        _hub = FindFirstObjectByType<Hub>();
    }

    private void Update()
    {
        if (_hub.Race.Car.IsFinished ||
            _hub.Race.Car.LapsCounter.IsWayCompleted ||
            _hub.Race.Car.Speed < 1)
        {
            _back.gameObject.SetActive(false);            
            return;
        }

        bool isPointBack = _hub.Race.Car.LapsCounter.RelativePointPosition.z < 0;
        if (!isPointBack)
        {
            _back.gameObject.SetActive(false);
            _backTime = 0;
            return;
        }

        _backTime += Time.deltaTime;
        _back.gameObject.SetActive(_backTime > _showTime);
    }
}
