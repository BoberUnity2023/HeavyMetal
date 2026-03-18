using UnityEngine;

public class SkidSound : MonoBehaviour
{
    [SerializeField] private Car _car;
    [SerializeField] private WheelSkid[] _wheelSkids = new WheelSkid[4];
    [SerializeField] private AudioSource _audioSource;
    //[SerializeField] private AudioClip _audioClip;

    void Update()
    {
        float volume = 0;
        foreach (var wheel in _wheelSkids)
        {
            volume += wheel.Intensity;
        }

        if (_car.Speed < 1)
            volume *= _car.Speed;

        _audioSource.volume = volume /= 4;
    }
}
