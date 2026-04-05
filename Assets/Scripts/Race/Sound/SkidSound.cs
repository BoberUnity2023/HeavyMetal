using UnityEngine;

public class SkidSound : MonoBehaviour
{
    [SerializeField] private Car _car;    
    [SerializeField] private AudioSource _audioSource;
    [Range(0, 1)][SerializeField] private float _scale;
    private WheelSkid[] _wheelSkids = new WheelSkid[4];

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Update_SetVolume();
    }

    private void Init()
    {
        for (int i = 0; i < 4; i++)
        {
            _wheelSkids[i] = _car.Wheels[i].GetComponent<WheelSkid>();
        }
    }

    private void Update_SetVolume()
    {
        float volume = 0;
        foreach (var wheel in _wheelSkids)
        {
            volume += wheel.Intensity;
        }

        if (_car.Speed < 1)
            volume *= _car.Speed;

        _audioSource.volume = Mathf.Clamp(volume / 4, 0, 1) * _scale;
    }
}
