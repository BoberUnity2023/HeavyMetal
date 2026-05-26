using System.Collections.Generic;
using UnityEngine;

public class Escalator : MonoBehaviour
{    
    [SerializeField] private int _speed;
    private List<Car> _cars = new List<Car>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            Car car = other.GetComponentInParent<Car>();
            SpeedUpStart(car);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Car")
        {
            Car car = other.GetComponentInParent<Car>();
            SpeedUpStop(car);
        }
    }

    private void SpeedUpStart(Car car)
    {
        car.IsOnEscalator = true;
        _cars.Add(car);
        enabled = true;
    }

    private void SpeedUpStop(Car car)
    {
        car.IsOnEscalator = false;
        _cars.Remove(car);
        if (_cars.Count == 0) 
            enabled = false;
    }

    private void FixedUpdate()
    {
        foreach (Car car in _cars) 
        {
            car.Rigidbody.AddForce(transform.forward * _speed);
        }
    }
}
