using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum WindowSettingsState
{
    MainMenu,
    Game
}

public class WindowSettings : MonoBehaviour
{
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private Hub _hub;

    [SerializeField] private Slider _sliderSoundVolume;
    [SerializeField] private GameObject _iconSoundOn;
    [SerializeField] private GameObject _iconSoundOff;

    [SerializeField] private Slider _sliderMusicVolume;
    [SerializeField] private GameObject _iconMusicOn;
    [SerializeField] private GameObject _iconMusicOff;

    [SerializeField] private Slider _sliderGrafic;

    [SerializeField] private Slider _sliderSpeed;
    [SerializeField] private Animator _speedIconAnimator;

    [SerializeField] private Slider _sliderJoystickAlpha;
    [SerializeField] private CanvasGroup _canvasGroupJoystickIcon;

    [SerializeField] private GameObject[] _testButtons;

    private bool _playingSound;
    private int _showConsolePressCount;    

    private GameController Game
    {
        get
        {
            if (State == WindowSettingsState.Game)
                return _hub.Game;

            if (State == WindowSettingsState.MainMenu)
                return _mainMenuController.Game;

            return null;
        }
    }

    private WindowSettingsState State
    {
        get
        {
            if (_hub != null)
                return WindowSettingsState.Game;

            if (_mainMenuController != null)
                return WindowSettingsState.MainMenu;

            return WindowSettingsState.MainMenu;
        }
    }    

    private void OnDestroy()
    {
        
    }

    private void OnEnable()
    {
        _sliderJoystickAlpha.value = PlayerPrefs.GetFloat("JoystickAlpha", 0.75f);
        OnValueChanged(PlayerPrefs.GetFloat("JoystickAlpha", 0.75f));
        
        _sliderMusicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 0.15f);
        OnMusicVolumeChanged(PlayerPrefs.GetFloat("MusicVolume", 0.15f));

        _sliderSoundVolume.value = PlayerPrefs.GetFloat("SoundVolume", 1);
        OnSoundVolumeChanged(PlayerPrefs.GetFloat("SoundVolume", 1));

        _sliderGrafic.value = PlayerPrefs.GetInt("QualityLevel", 1);
        OnGraficChanged(PlayerPrefs.GetInt("QualityLevel", 1));

        _sliderSpeed.value = PlayerPrefs.GetFloat("HeroSpeed", 0.4f);
        //OnGraficChanged(PlayerPrefs.GetInt("QualityLevel", 1));

        Game.Sound.OnMuteSFX += OnMuteSFX;
        Game.Sound.OnMuteMusic += OnMuteMusic;
        Game.Sound.OnUnmuteSFX += OnUnmuteSFX;
        Game.Sound.OnUnmuteMusic += OnUnmuteMusic;
    }

    private void OnDisable()
    {
        Game.Sound.OnMuteSFX -= OnMuteSFX;
        Game.Sound.OnMuteMusic -= OnMuteMusic;
        Game.Sound.OnUnmuteSFX -= OnUnmuteSFX;
        Game.Sound.OnUnmuteMusic -= OnUnmuteMusic;
    }

    public void PressTimeStep(int frameRate)//Do not use
    {
        Application.targetFrameRate = frameRate;
        PlayerPrefs.SetInt("Framerate", frameRate);
    }

    public void PressBack() 
    {
        Game.Sound.Play(SoundClip.Click);
        gameObject.SetActive(false);
        if (State == WindowSettingsState.Game)
        {
            _hub.CanvasLevel.PausePanelShow();            
        }        
    }    

    public void PressResetSaveProgress()
    {
        Debug.Log("PressResetSaveProgress");        
        //Game.Saves.ResetProgress();
    }

    public void PressOpenAllLevels()//DoNotUse?
    {
        Debug.Log("OpenAllLevels");
        for (int i = 0; i < 95; i++)
        {
            //Game.Saves.SetLevelStars(i, 3);
        }        
    }    

    public void OnValueChanged(float value)
    {
        PlayerPrefs.SetFloat("JoystickAlpha", value);
        PlayerPrefs.Save();
        _canvasGroupJoystickIcon.alpha = value;
        if (State == WindowSettingsState.Game)
            _hub.Joistick.SetAlpha();

        TryPlaySound();
    }

    public void OnSoundVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();
        _iconSoundOn.SetActive(value > 0);
        _iconSoundOff.SetActive(value == 0);
        TryPlaySound();
    }       

    public void OnMusicVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        Game.Sound.SetMusicVolume(value);
        _iconMusicOn.SetActive(value > 0);
        _iconMusicOff.SetActive(value == 0);
        TryPlaySound();
    }

    public void OnGraficChanged(float value)
    {
        PlayerPrefs.SetInt("QualityLevel", (int)value);
        PlayerPrefs.Save();

        if (State == WindowSettingsState.Game)
        {
            _hub.Optimization.Set();
            _hub.Game.Settings.SetGrafics();
        }
        if (State == WindowSettingsState.MainMenu)
        {
            _mainMenuController.Game.Settings.SetGrafics();
        }

        TryPlaySound();
    }

    public void OnSpeedChanged(float value)
    {
        PlayerPrefs.SetFloat("HeroSpeed", value);
        PlayerPrefs.Save();

        _speedIconAnimator.speed = value + 0.5f;

        if (State == WindowSettingsState.Game)
        {            
            //_hub.Hero.Move.SetSpeed();
        }
        if (State == WindowSettingsState.MainMenu)
        {
            
        }

        TryPlaySound();
    }

    public void PressShowConsole()
    {
        _showConsolePressCount++;
        if (_showConsolePressCount < 12)
            return;

        Game.Console.enabled = true;
        foreach (var item in _testButtons)
        {
            item.SetActive(true);
        }
    }

    public void PressCheatMode()
    {
        //_cheatModePressed++;
        //if (_cheatModePressed < 8)//TODO:
        //    return;

        //Debug.Log("CheatMode enabled");
        //Game.CheatMode = true;
    }

    private void TryPlaySound()
    {
        if (_playingSound)
            return;

        _playingSound = true;
        Game.Sound.Play(SoundClip.Click);
        StopAllCoroutines();
        float time = Game.Sound.ClipLength(SoundClip.Click);
        StartCoroutine(AterPlaySound(time));
    }

    private IEnumerator AterPlaySound(float time)
    {
        yield return new WaitForSeconds(time);
        _playingSound = false;
    }
    
    private void OnMuteSFX()
    {
        OnSoundVolumeChanged(0);
    }

    private void OnMuteMusic()
    {
        OnMusicVolumeChanged(0);
    }

    private void OnUnmuteSFX()
    {
        OnSoundVolumeChanged(1);
    }

    private void OnUnmuteMusic()
    {
        OnMusicVolumeChanged(0.15f);
    }   
}
