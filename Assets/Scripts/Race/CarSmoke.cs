using UnityEngine;

public class CarSmoke : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private ParticleSystem[] _smokes = null;
    [SerializeField] private float _rateMin;
    [SerializeField] private float _rateMax;

    private void Update()
    {
        Update_SetEmit();
    }

    private void Update_SetEmit()
    {
        foreach (var smoke in _smokes)
        {
            ParticleSystem.EmissionModule emissionModule = smoke.emission;                    
            emissionModule.rateOverTime = RateOverTime;            
        }
    }

    private float RateOverTime
    {
        get
        {
            if (!_car.Control.IsAccelerating)
                return _rateMin;

            return _rateMin + (_rateMax - _rateMin) * Mathf.Abs(_car.Input.Force);
        }
    }
}
