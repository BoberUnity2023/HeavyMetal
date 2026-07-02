using UnityEngine;

public class SkidSound : VolumeSound
{    
    [SerializeField] private AudioSource _audioSource;
    [Range(0, 1)][SerializeField] private float _scale;    

    private void Update()
    {
        Update_SetVolume();
    }

    public override void Init(Car car)
    {
        base.Init(car);        
    }

    private void Update_SetVolume()
    {
        _audioSource.volume = _car.SlideForce * _scale * _volumeSound;
    }
}
