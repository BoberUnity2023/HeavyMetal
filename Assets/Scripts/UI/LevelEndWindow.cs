using UnityEngine;
using UnityEngine.UI;

using TMPro;


public class LevelEndWindow : MonoBehaviour
{
    [SerializeField] CanvasLevel _canvasLevel;    
    [SerializeField] private GameObject _window;    
    [SerializeField] private Button _buttonRestartFromCheckpoint;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private Button _buttonNextLevel;
    [SerializeField] private GameObject _iconVideo;
    [SerializeField] private GameObject _iconButterfly;
    [SerializeField] private TMP_Text _butterflyIndicator;

    private Hub _hub;

    private void Start()
    {
        _hub = _canvasLevel.Hub;
        _hub.Level.OnLevelComplete += OnLevelComplete;
        _hub.Level.OnLevelLost += OnLevelLost;
        _hub.Level.OnLevelRestartFromCheckpoint += OnLevelRestartFromCheckpoint;
    }

    private void OnDestroy()
    {
        _hub.Level.OnLevelComplete -= OnLevelComplete;
        _hub.Level.OnLevelLost -= OnLevelLost;
        _hub.Level.OnLevelRestartFromCheckpoint -= OnLevelRestartFromCheckpoint;
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
        Show();        
        _buttonNextLevel.gameObject.SetActive(true);
        _buttonRestart.gameObject.SetActive(false);
        _buttonRestartFromCheckpoint.gameObject.SetActive(false);
    }

    private void OnLevelLost()
    {
        Show();
        _buttonNextLevel.gameObject.SetActive(false);
        _buttonRestart.gameObject.SetActive(true);
        _buttonRestartFromCheckpoint.gameObject.SetActive(_hub.Level.HasCheckpoint);
        
    }

    private void OnLevelRestartFromCheckpoint()
    {
        Hide();
    }
}
