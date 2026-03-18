using UnityEngine;

public class SkidSound : MonoBehaviour
{
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

        //_audioSource.clip = _audioClip;
        _audioSource.volume = volume /= 4;
    }
}
