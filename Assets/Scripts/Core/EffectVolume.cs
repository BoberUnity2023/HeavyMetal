using UnityEngine;

public class EffectVolume : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;

    private void Start()
    {        
        float volume = _audioSource.volume;
        _audioSource.volume = volume * PlayerPrefs.GetFloat("SoundVolume");
    }
}
