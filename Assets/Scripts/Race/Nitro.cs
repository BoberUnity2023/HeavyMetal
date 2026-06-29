using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Nitro : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particleSystems;
    [SerializeField] private AudioSource _audioSource;
    //[Range(0, 1)][SerializeField] private float _scale;
    [SerializeField] private float _time;
    [SerializeField] private float _multipler;
    [SerializeField] private float _fullTime;
    private float _fill;
    private Car _car;
    private Coroutine _coroutine;
    private bool _isOn;

    public bool IsOn => _isOn;

    public float Multipler => _multipler;

    public float FillProgress => _fill / _fullTime;

    public void Init(Car car)
    {
        _car = car;
        enabled = false;
    }

    private void Update()
    {
        _fill -= Time.deltaTime;
        if (_fill < 0)
        {
            Off();
        }
    }

    public void AddTuningTime(float time)
    {
        _fullTime += time;
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

    public void Add()
    {
        _fill += 1;
        if (_fill > _fullTime)
            _fill = _fullTime;
    }

    public void OnManual()
    {
        if (_coroutine != null)
            StopCoroutine(_coroutine);

        if (_fill > 0)
        {
            _isOn = true;
            enabled = true;
            SetEmission(true);
            //_audioSource.volume = _car.Hub.Game.Sound.SoundVolume * _scale;
            _audioSource.Play();
        }
    }

    public void OnAuto()
    {
        OnManual();
        _coroutine = StartCoroutine(NitroOff(_time));
    }

    private IEnumerator NitroOff(float time)
    {
        yield return new WaitForSeconds(time);
        Off();
    }

    public void Off()
    {
        if (!_isOn)
            return;

        _isOn = false;
        enabled = false;
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
