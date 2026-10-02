using UnityEngine;

public class CarSmoke : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private ParticleSystem[] _smokes = null;
    [SerializeField] private float _rateMin;
    [SerializeField] private float _rateMax;
    private int _smokesCount;
    
    private void Start()
    {        
        if (_smokes != null)
            _smokesCount = _smokes.Length;
    }

    private void Update()
    {
        if (!_car.IsVisible)
            return;

        Update_SetEmit();
    }

    private void Update_SetEmit()
    {
        for (int i = 0; i < _smokesCount; i++)
        {
            ParticleSystem.EmissionModule emissionModule = _smokes[i].emission;                    
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
