using UnityEngine;
using UnityEngine.UI;

public class GarageSettingsMenu : MonoBehaviour
{
    [SerializeField] private SceneController _sceneController;
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
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
        _sceneController.Game.Sound.SetMusicVolume(value);
        _iconMusicOn.SetActive(value > 0);
        _iconMusicOff.SetActive(value == 0);        
    }

    public void OnGraficChanged(float value)
    {
        PlayerPrefs.SetInt("QualityLevel", (int)value);
        PlayerPrefs.Save();

        _sceneController.Game.Settings.SetGrafics();
    }
}
