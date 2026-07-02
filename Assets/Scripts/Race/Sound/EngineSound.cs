using UnityEngine;

public class EngineSound : VolumeSound
{    
    [SerializeField] private AudioSource _audioSource;
    [Range(0, 1)][SerializeField] private float _scale;

    private bool _isFun;
    private float _volume;
    private float _volumeTarget;
    private float _pitch;
    private float _pitchTarget;

    private void Update()
    {        
        if (_car.Force < 0.05f)
        {
            _volumeTarget = 0.5f * _scale; ;

            if (_isFun)
            {
                _pitchTarget = 0.75f;                
            }
            _isFun = false;
        }
        else
        {
            _volumeTarget = _car.Force * _scale; ;

            if (!_isFun)
            {
                _pitchTarget = 1.25f;
            }

            _isFun = true;
        }

        _volume = Mathf.Lerp(_volume, _volumeTarget, Time.deltaTime*3);
        _pitch = Mathf.Lerp(_pitch, _pitchTarget, Time.deltaTime*3);
        _audioSource.volume = _volume;
        _audioSource.pitch = _pitch;
    }
}
