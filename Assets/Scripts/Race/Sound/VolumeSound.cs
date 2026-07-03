using UnityEngine;

public class VolumeSound : MonoBehaviour
{
    protected Car _car;
    protected float _volumeSound;
    private bool _isInited;

    public virtual void Init(Car car)
    {
        _car = car;        

        if (_car.Mode == Mode.Track)
        {
            enabled = true;
            //_car.Hub.CanvasLevel.OnSettingsClose += OnSettingsClose;
            GetVolumeSound();
            _isInited = true;
        }
    }    

    protected virtual void OnDestroy()
    {
        //if (_isInited)
        //    _car.Hub.CanvasLevel.OnSettingsClose -= OnSettingsClose;
    }

    private void OnSettingsClose()
    {
        GetVolumeSound();
    }

    private void GetVolumeSound()
    {
        _volumeSound = PlayerPrefs.GetFloat("SoundVolume", 1);
    }
}
