using UnityEngine;
using UnityEngine.UI;

public class GarageSettingsMenu : WindowBase
{
    [SerializeField] private Garage _garage;
    [SerializeField] private Slider _sliderMusicVolume;
    [SerializeField] private GameObject _iconMusicOn;
    [SerializeField] private GameObject _iconMusicOff;

    [SerializeField] private Slider _sliderQuality;    

    private void OnEnable()
    {   
        _sliderMusicVolume.value = PlayerPrefs.GetFloat("MusicVolume", 0.15f);

        _sliderQuality.value = PlayerPrefs.GetInt("QualityLevel", 2);
        OnGraficChanged(PlayerPrefs.GetInt("QualityLevel", 2));
    }

    public void OnMusicVolumeChanged(float value)
    {
        Game.Sound.Play(SoundClip.Click);
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        _sceneController.Game.Sound.SetMusicVolume(value);
        _iconMusicOn.SetActive(value > 0);
        _iconMusicOff.SetActive(value == 0);        
    }

    public void OnGraficChanged(float value)
    {
        Game.Sound.Play(SoundClip.Click);
        PlayerPrefs.SetInt("QualityLevel", (int)value);
        PlayerPrefs.Save();

        _sceneController.Game.Settings.SetGrafics();
    }

    public void PressClose()
    {
        Game.Sound.Play(SoundClip.Click);
        Hide();
        _garage.MainMenu.Show();
    }

    public void OnLanguageChanged(int id)
    {
        Debug.Log("Lang:" + id);
        Game.Sound.Play(SoundClip.Click);
        if (id == 0)        
            _sceneController.Game.Localize.ChangeLanguage("en");

        if (id == 1)
            _sceneController.Game.Localize.ChangeLanguage("ru");

        if (id == 2)
            _sceneController.Game.Localize.ChangeLanguage("es");

        if (id == 3)
            _sceneController.Game.Localize.ChangeLanguage("fr");

        if (id == 4)
            _sceneController.Game.Localize.ChangeLanguage("de");

        if (id == 5)
            _sceneController.Game.Localize.ChangeLanguage("en");

        if (id == 7)
            _sceneController.Game.Localize.ChangeLanguage("en");

    }
}
