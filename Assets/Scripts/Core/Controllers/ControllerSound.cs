using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SoundClip
{
    Click,
    Checkpoint,
    WaterSplash,
    Jump,
    JumpEnd,
    CakeCrash,
    PodnosCollision,
    LevelComplete,
    FootStepGround,
    FootStepWater,
    Mushroom,
    ButterflyOut,
    BatDead
}

public class ControllerSound : MonoBehaviour
{
    [SerializeField] private GameController _game;
    [SerializeField] private AudioSource _audioSourceMusic;
    [SerializeField] private AudioClip _click;
    [SerializeField] private AudioClip _checkpoint;
    [SerializeField] private AudioClip _waterSplash;
    [SerializeField] private AudioClip _clipJump;
    [SerializeField] private AudioClip _clipJumpEnd;
    [SerializeField] private AudioClip _clipCakeCrash;
    [SerializeField] private AudioClip _podnosCollision;
    [SerializeField] private AudioClip _levelComplete;
    [SerializeField] private AudioClip _clipFootStepGround;
    [SerializeField] private AudioClip _clipFootStepWater;
    [SerializeField] private AudioClip _mushroom;
    [SerializeField] private AudioClip _butterflyOut;
    [SerializeField] private AudioClip _batDead;
    [SerializeField] private AudioClip[] _musics;
    private bool _isLocked;
    //private int _musicClip;

    public event Action OnMuteSFX;
    public event Action OnMuteMusic;
    public event Action OnUnmuteSFX;
    public event Action OnUnmuteMusic;

    public bool IsOn
    {
        get
        {
            return PlayerPrefs.GetInt("SoundVolume", 1) > 0;
        }        
    }

    public AudioClip ClipFootStepGround => _clipFootStepGround;

    public AudioClip ClipFootStepWater => _clipFootStepWater;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnApplicationFocus(bool hasFocus)
    {
        if (hasFocus)
            MusicPlay();
        else
            MusicPause();
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            MusicPause();
        else
            MusicPlay();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    { 
        SetMusicVolume(PlayerPrefs.GetFloat("MusicVolume", 0.15f));

        if (scene.buildIndex == 1)
        {
            SetMusicClip(0);
        }
        if (scene.buildIndex > 1)
        {
            SetMusicClip(1);
        }
    }

    public void MusicPause()
    {
        //if (_game.Platform == Platform.Vk && _game.Device == Device.Mobile)
        //    return;

        _audioSourceMusic.Pause();
    }

    public void MusicPlay()
    {
        if (_game.Platform == Platform.Vk && _game.Device == Device.Mobile)
            return;

        _audioSourceMusic.Play();
    }

    public void SetMusicVolume(float volume)
    {        
        _audioSourceMusic.volume = volume * 0.5f;
    }    

    public AudioSource Play(SoundClip soundClip)//void
    {
        if (!IsOn || !_game.HasFocus)
            return null;

        if (_isLocked)
            return null;

        try
        {
            GameObject sound = new GameObject();
            DontDestroyOnLoad(sound.gameObject);
            AudioSource audioSource = sound.AddComponent<AudioSource>();
            audioSource.clip = Clip(soundClip);
            audioSource.volume = PlayerPrefs.GetFloat("SoundVolume", 1);
            audioSource.Play();
            Destroy(sound, audioSource.clip.length);
            return audioSource;
        }
        catch(Exception exceprtion)
        {
            
            return null;
        }
    }

    public void Play(SoundClip soundClip, float volume, float speed)//Do not used
    {
        AudioSource audioSource = Play(soundClip);
        audioSource.volume = volume;
        audioSource.pitch = speed;
    }

    public float ClipLength(SoundClip soundClip)//Do not used
    {
        AudioClip clip = Clip(soundClip);
        return clip.length;
    }

    private AudioClip Clip(SoundClip soundClip)
    {
        switch (soundClip) 
        {
            case SoundClip.Click:
                return _click;

            case SoundClip.Checkpoint:
                return _checkpoint;

            case SoundClip.WaterSplash:
                return _waterSplash;

            case SoundClip.Jump:
                return _clipJump;

            case SoundClip.JumpEnd:
                return _clipJumpEnd;

            case SoundClip.CakeCrash: 
                return _clipCakeCrash;

            case SoundClip.PodnosCollision: 
                return _podnosCollision;

            case SoundClip.LevelComplete:
                return _levelComplete;

            case SoundClip.FootStepGround:
                return _clipFootStepGround;

            case SoundClip.FootStepWater:
                return _clipFootStepWater;

            case SoundClip.Mushroom:
                return _mushroom;

            case SoundClip.ButterflyOut: 
                return _butterflyOut;

            case SoundClip.BatDead:
                return _batDead;
        }
        return null;
    }

    private void SetMusicClip(int id)
    {
        if (_game.Platform == Platform.Vk && _game.Device == Device.Mobile)
        {
            _audioSourceMusic.Stop();
            return; 
        }
        
        //_musicClip++;
        //if (_musicClip >= _musics.Length)
        //    _musicClip = 0;        
        
        _audioSourceMusic.Stop();
        _audioSourceMusic.clip = _musics[id];
        _audioSourceMusic.Play();        
    }

    public void Lock()
    {
        _isLocked = true;
        _audioSourceMusic.enabled = false;         
    }

    public void UnLock()
    {
        _isLocked = false;
        _audioSourceMusic.enabled = true;        
    }
}
