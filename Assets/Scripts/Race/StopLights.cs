using UnityEngine;

public class StopLights : MonoBehaviour
{    
    [SerializeField] private Light[] _lights;
    [SerializeField] private float _intensityMin;
    [SerializeField] private float _intensityMax;
    private Car _car;
    private int _lightsCount;
    private bool _wasReversing;
    private bool _wasBraking;
    private bool _isFirstFrame = true;

    public void Init(Car car)
    {
        _car = car;
        _lightsCount = _lights.Length;

        if (_car.Mode == Mode.Track)
            enabled = true;
    }

    private void Update()
    {
        if (!_car.IsVisible)
            return;

        bool isReversing = _car.Input.Reverse > 0;
        bool isBraking = _car.Input.Brake > 0;

        if (_isFirstFrame || isReversing != _wasReversing || isBraking != _wasBraking)
        {
            _isFirstFrame = false;
            _wasReversing = isReversing;
            _wasBraking = isBraking;

            // Только если состояние ИЗМЕНИЛОСЬ, один раз обновляем фары
            UpdateLightsState(isReversing, isBraking);
        }        
    }

    private void UpdateLightsState(bool isReversing, bool isBraking)
    {
        for (int i = 0; i < _lightsCount; i++)
        {
            Light light = _lights[i];
            if (light == null) continue;

            if (isReversing)
            {
                light.color = Color.white;
                light.intensity = _intensityMax;
            }
            else
            {
                light.color = Color.red;
                light.intensity = isBraking ? _intensityMax : _intensityMin;
            }
        }
    }
}
