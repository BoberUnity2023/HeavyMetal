using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LevelEndWindow : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    [SerializeField] private GameObject _window;
    [SerializeField] private Text _finishText;
    [SerializeField] private Button _buttonRestartFromCheckpoint;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonNextLevel;
    [SerializeField] private GameObject _iconVideo;    

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
        _finishText.gameObject.SetActive(true);
        _finishText.text = _hub.Place.Place.ToString();
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
