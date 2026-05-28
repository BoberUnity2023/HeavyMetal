using System;
using UnityEngine;

public class CarOil : MonoBehaviour
{
    private Car _car;
    [SerializeField] private float _oilDistance;
    [SerializeField] private float _oilDistanceCurrent;       

    public float Intensity => _oilDistanceCurrent / _oilDistance;      

    private void FixedUpdate()
    {
        float minPath = Mathf.Abs(_car.Force) * 10 * Time.fixedDeltaTime;//10 buksSpeed;
        float path = Mathf.Abs(_car.Speed) * Time.fixedDeltaTime;
        path = Mathf.Max(path, minPath);

        _oilDistanceCurrent -= path;        

        if (_oilDistanceCurrent < 0)
            OilEnd();
        else
            FixedUpdate_SetWheels(Mathf.Sqrt(Intensity));        
    }

    public void Init(Car car)
    {
        _car = car;
        enabled = false;
    }

    public void OilStart(float distance)
    {
        _oilDistance = distance;
        _oilDistanceCurrent = distance;
        enabled = true;
        
        float force = _car.LocalVelocity.x * 500000;
        force *= _car.SpeedFactor;

        _car.Rigidbody.AddForceAtPosition(transform.right * force, transform.position + transform.forward * 2);
        _car.Rigidbody.AddForceAtPosition(-transform.right * force, transform.position - transform.forward * 2);
    }

    public void OilEnd()
    {
        enabled = false;
        foreach (WheelSkid wheelSkid in _car.WheelSkids)
        {
            wheelSkid.SetFriction();
            wheelSkid.SetSkidColor();
        }
    }

    private void FixedUpdate_SetWheels(float intensity)
    {
        foreach (WheelSkid wheelSkid in _car.WheelSkids)
        {
            wheelSkid.SetFrictionWithIntensity(1 - intensity);            
            wheelSkid.SetSkidColor(Color.yellow);
        }
    }
}
