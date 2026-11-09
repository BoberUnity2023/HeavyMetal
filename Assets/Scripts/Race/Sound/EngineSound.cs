using UnityEngine;

public class EngineSound : VolumeSound
{
    [SerializeField] private AudioSource _audioSource;
    [Range(0, 1)][SerializeField] private float _scale;

    private Car _playerCar;
    private float _volume;
    private float _volumeTarget;
    private float _pitch;
    private float _pitchTarget;
    private float _maxDistanceSqr;

    private void Start()
    {
        _playerCar = _car.Hub.Level.Race.Car;
        _maxDistanceSqr = _audioSource.maxDistance * _audioSource.maxDistance;
    }

    private void Update()
    {
        Update_EngineSound();
    }

    private void Update_EngineSound()
    {
        float sqrDistance = (transform.position - _playerCar.transform.position).sqrMagnitude;
        if (sqrDistance > _maxDistanceSqr)
        {
            if (_audioSource.volume > 0)
                _audioSource.volume = 0;

            return;
        }

        if (_car.Force < 0.05f)
        {
            _volumeTarget = 0.4f * _scale;
            // ћашина катитс€ или стоит Ч звук холостого хода (низкий тон)
            _pitchTarget = 0.75f;
        }
        else
        {
            _volumeTarget = _car.Force * _scale;
            // »ƒ≈јЋ№Ќќ ƒЋя √ќЌќ : ƒинамический питч! 
            // «вук мотора будет плавно повышатьс€ от 0.85 до 1.35 в зависимости от силы нажати€ газа!
            _pitchTarget = Mathf.Lerp(0.85f, 1.35f, _car.Force);
        }

        // 3. ѕлавный и стерильный Lerp в стеке пам€ти процессора
        _volume = Mathf.Lerp(_volume, _volumeTarget, Time.deltaTime * 3);
        _pitch = Mathf.Lerp(_pitch, _pitchTarget, Time.deltaTime * 3);

        _audioSource.volume = _volume * _volumeSound;
        _audioSource.pitch = _pitch;
    }
}
