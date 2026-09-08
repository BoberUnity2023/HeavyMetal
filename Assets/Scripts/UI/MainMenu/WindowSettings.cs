using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum WindowSettingsState
{
    MainMenu,
    Game
}

public class WindowSettings : WindowBase
{
    [SerializeField] private Garage _garage;
    [SerializeField] private Hub _hub;
    [SerializeField] private Slider _sliderMusicVolume;
    [SerializeField] private Slider _sliderSoundVolume;
    [SerializeField] private GameObject _iconMusicOn;
    [SerializeField] private GameObject _iconMusicOff;
    [SerializeField] private GameObject _iconSoundOn;
    [SerializeField] private GameObject _iconSoundOff;

    [SerializeField] private Slider _sliderQuality;
    private GameController _game;
    private bool _playingSound;

    public event Action OnSettingsClose;

    public override void Init(GameController game)
    {
        base.Init(game);
        _game = game;
        _sliderMusicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 0.15f);
        OnMusicVolumeChanged(PlayerPrefs.GetFloat("MusicVolume", 0.15f));
        _sliderSoundVolume.value = PlayerPrefs.GetFloat("SoundVolume", 1);
        OnSoundVolumeChanged(PlayerPrefs.GetFloat("SoundVolume", 1));

        _sliderQuality.value = PlayerPrefs.GetInt("QualityLevel", 2);
        OnGraficChanged(PlayerPrefs.GetInt("QualityLevel", 2));
    }

    public void OnSoundVolumeChanged(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        PlayerPrefs.Save();
        _game.Sound.VolumeSound = value;
        _iconSoundOn.SetActive(value > 0);
        _iconSoundOff.SetActive(value == 0);
        TryPlaySound();
    }

    public void OnMusicVolumeChanged(float value)
    {
        _game.Sound.Play(SoundClip.Click);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        _game.Sound.SetMusicVolume(value);
        _iconMusicOn.SetActive(value > 0);
        _iconMusicOff.SetActive(value == 0);
    }

    public void OnGraficChanged(float value)
    {
        _game.Sound.Play(SoundClip.Click);
        PlayerPrefs.SetInt("QualityLevel", (int)value);
        PlayerPrefs.Save();

        _game.Settings.SetGrafics();
    }

    public void PressClose()
    {
        _game.Sound.Play(SoundClip.Click);
        Hide();
        if (_garage != null)
            _garage.MainMenu.Show();
        else
        {            
            _hub.Game.UI.NavigationEnd();
            _hub.CanvasLevel.SettingsClose();
        }
    }

    public void OnLanguageChanged(int id)
    {
        Debug.Log("Lang:" + id);
        _game.Sound.Play(SoundClip.Click);
        if (id == 0)
            _game.Localize.ChangeLanguage("en");

        if (id == 1)
            _game.Localize.ChangeLanguage("ru");

        if (id == 2)
            _game.Localize.ChangeLanguage("es");

        if (id == 3)
            _game.Localize.ChangeLanguage("fr");

        if (id == 4)
            _game.Localize.ChangeLanguage("de");        

        if (id == 5)
            _game.Localize.ChangeLanguage("pt");
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
}
