using System.Collections;
using UnityEngine;

public class Nitro : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _time;
    [SerializeField] private float _multipler;
    private Car _car;
    private Coroutine _coroutine;
    private bool _isOn;

    public bool IsOn => _isOn;

    public float Multipler => _multipler;

    public void Init(Car car)
    {
        _car = car;
    }
    
    public void On()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        SetEmission(true);
        _coroutine = StartCoroutine(NitroOff(_time));
        //_audioSource.volume = _car.Hub.Game.Sound.SoundVolume;
        _audioSource.Play();
    }

    private IEnumerator NitroOff(float time)
    {
        yield return new WaitForSeconds(time);
        Off();
    }

    private void Off()
    {
        SetEmission(false);
    }

    private void SetEmission(bool value)
    {
        _isOn = value;

        foreach (ParticleSystem particleSystem in _particleSystems)
        {
            ParticleSystem.EmissionModule _emissionModules = particleSystem.emission;
            _emissionModules.enabled = value;            
        }
    }
}
