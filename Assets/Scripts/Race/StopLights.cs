using UnityEngine;

public class StopLights : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private Light[] _lights;
    [SerializeField] private float _intensityMin;
    [SerializeField] private float _intensityMax;

    private void Update()
    {
        foreach (var light in _lights) 
        {            
            if (_car.Input.Reverse > 0)
            {
                light.color = Color.white;
                light.intensity = _intensityMax;
            }
            else
            {
                light.color = Color.red;
                light.intensity = _car.Input.Brake > 0 ? _intensityMax : _intensityMin;
            }
        }
    }
}
