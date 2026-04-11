using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndWindow : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    [SerializeField] private GameObject _window;
    [SerializeField] private TMP_Text _finishText;
    [SerializeField] private Button _buttonRestartFromCheckpoint;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonNextLevel;
    [SerializeField] private Button _buttonGarage;
    [SerializeField] private GameObject _iconVideo;
    [SerializeField] private GameObject[] _stars;

    private void Start()
    {        
        Hide();        
        StartCoroutine(AfterStart(1));
    }

    private IEnumerator AfterStart(float time)
    {
        yield return new WaitForSeconds(time);
        _hub.Level.Race.OnFinish += Race_OnFinish;
    }

    private void Race_OnFinish()
    {
        Show();
        _buttonGarage.gameObject.SetActive(false);
        _finishText.gameObject.SetActive(true);
        int place = _hub.Result.Place;
        _finishText.text = place.ToString();
        _stars[0].SetActive(place <= 3);
        _stars[1].SetActive(place <= 2);
        _stars[2].SetActive(place == 1);
    }

    private void OnDestroy()
    {
        _hub.Level.Race.OnFinish -= Race_OnFinish;
    }

    public void Show()
    {
        _window.gameObject.SetActive(true);
    }

    public void Hide()
    {
        _window.gameObject.SetActive(false);
    }

    private void OnLevelComplete(int cakes)
    {        
        //_buttonNextLevel.gameObject.SetActive(true);
        //_buttonRestart.gameObject.SetActive(false);
        //_buttonRestartFromCheckpoint.gameObject.SetActive(false);
    }

    private void OnLevelLost()
    {
        Show();
        _buttonNextLevel.gameObject.SetActive(false);
        _buttonRestart.gameObject.SetActive(true);
        //_buttonRestartFromCheckpoint.gameObject.SetActive(_hub.Level.HasCheckpoint);        
    }

    private void OnLevelRestartFromCheckpoint()
    {
        Hide();
    }
}
