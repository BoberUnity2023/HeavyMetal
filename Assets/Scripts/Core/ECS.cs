using UnityEngine;

public class ECS : MonoBehaviour
{
    [SerializeField] private Hub _hub;

    private void FixedUpdate()
    {
        //if (_hub.Level.IsRace)
        //    _hub.Level.Race.Car.CarControl.ECS_FixedUpdate();

        //_hub.Camera.ECS_FixedUpdate_TryMove();
    }
}
