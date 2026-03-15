using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CanvasLevel : MonoBehaviour
{
    [SerializeField] private Hub _hub;    
    [SerializeField] private GameObject _buttonOpenPanel;
    [SerializeField] private GameObject _buttonPanelCloseScreen;
    [SerializeField] private GameObject _buttonCamera;
    [SerializeField] private GameObject _buttonMainMenu;
    [SerializeField] private GameObject _buttonOptions;
    [SerializeField] private GameObject _buttonRestart;
    [SerializeField] private GameObject _jump;
    [SerializeField] private Animator _buttonJumpAnimator;
    [SerializeField] private TMP_Text _scoreIndicator;
    [SerializeField] private GameObject _butterfly;
    [SerializeField] private TMP_Text _butterflyIndicator;
    [SerializeField] private GameObject _buttonForce;
    [SerializeField] private GameObject _buttonBrake;
    [SerializeField] private GameObject _buttonHandbrake;
    [SerializeField] private GameObject _buttonLeft;
    [SerializeField] private GameObject _buttonRight;
    private int _scoreStartLevel;

    public Hub Hub => _hub;

    public event Action OnPressRestart;
    public event Action OnPressRestartFromCheckpoint;
    public event Action OnPressPanelOpen;

    private void Start()
    {
        _hub.Level.OnLevelComplete += Level_OnLevelComplete;
        _hub.Level.OnLevelLost += Level_OnLevelLost;
        _hub.Level.OnLevelRestartFromCheckpoint += Level_OnLevelRestartFromCheckpoint;
        //_hub.Game.OnScoreChanged += Game_OnScoreChanged;
        _jump.SetActive(_hub.Game.Device == Device.Mobile);
        _scoreStartLevel = _hub.Game.Score;
        PausePanelHide();

        if (_hub.Level.IsRace)
            RaceButtonsShow();
        else
            RaceButtonsHide();
    }

    private void OnDestroy()
    {
        _hub.Level.OnLevelComplete -= Level_OnLevelComplete; 
        _hub.Level.OnLevelLost -= Level_OnLevelLost; 
        _hub.Level.OnLevelRestartFromCheckpoint -= Level_OnLevelRestartFromCheckpoint;
        //_hub.Game.OnScoreChanged -= Game_OnScoreChanged;
    }

    public void PausePanelShow()
    {
        Debug.Log("PausePanelShow()");
        _buttonPanelCloseScreen.SetActive(true);
        _buttonCamera.SetActive(true);
        _buttonMainMenu.SetActive(true);
        _buttonOptions.SetActive(true);
        _buttonRestart.SetActive(_hub.Level.IsPlaying);
        _butterfly.SetActive(false);
        if (_hub.Level.IsRace) 
            RaceButtonsHide();
        OnPressPanelOpen?.Invoke();
    }

    public void PausePanelHide()
    {
        _buttonPanelCloseScreen.SetActive(false);
        _buttonCamera.SetActive(false);
        _buttonMainMenu.SetActive(false);
        _buttonOptions.SetActive(false);
        _buttonRestart.SetActive(false);
        //_butterfly.SetActive(_hub.Game.Saves.Butterflies > 0 && _hub.Level.IsPlaying);
        if (_hub.Level.IsRace/* && _hub.Game.Device == Device.Mobile*/)
            RaceButtonsShow();
    }

    public void PressMainMenu()
    {
        _hub.Sound.PressSound();
        _hub.Input.LoadScene(1);        
    }

    public void PressRestart()
    {               
        _hub.Sound.PressSound();
        OnPressRestart?.Invoke();
        StartCoroutine(AfterPressRestart());
    }

    private IEnumerator AfterPressRestart()
    {
        yield return new WaitForSeconds(0.1f);
        _hub.Input.LoadLevelRestart();
    }

    public void PressNextLevel()
    {        
        _hub.Sound.PressSound();
        StartCoroutine(AfterPressNextLevel());
    }

    private IEnumerator AfterPressNextLevel()
    {
        yield return new WaitForSeconds(0.1f);
        _hub.Input.LoadLevelNext(); 
    }

    public void PressRestartFromCheckpoint()
    {
        _hub.Sound.PressSound();
        OnPressRestartFromCheckpoint?.Invoke();              
    }

    public void PressCamera()
    {
        _hub.Sound.PressSound();
        _hub.Camera.SetNextTargetState();
    }

    public void PressSettings()
    {
        _hub.Sound.PressSound();
        _hub.WindowSettings.gameObject.SetActive(true);
        PausePanelHide();
    }

    public void PressPanelOpen()
    {
        _hub.Sound.PressSound();
        _buttonOpenPanel.SetActive(false);
        PausePanelShow();
        _jump.SetActive(false);
        _hub.Joistick.ResetDelta();
        _hub.Joistick.ResetCenter();
        _hub.Joistick.gameObject.SetActive(false);
        enabled = true;
    }

    public void PressPanelClose()
    {
        _hub.Sound.PressSound();
        _buttonOpenPanel.SetActive(true);
        PausePanelHide();
        _jump.SetActive(_hub.Game.Device == Device.Mobile && _hub.Level.IsPlaying);
        _hub.Joistick.gameObject.SetActive(true);
        enabled = false;
    }

    public void PointerDownLeft()
    {
        _hub.Input.PlayerInput.PointerDownLeft();
    }

    public void PointerUpLeft()
    {
        _hub.Input.PlayerInput.PointerUpLeft();
    }

    public void PointerDownRight()
    {
        _hub.Input.PlayerInput.PointerDownRight();
    }

    public void PointerUpRight()
    {
        _hub.Input.PlayerInput.PointerUpRight();
    }

    public void PointerDownForce()
    {
        _hub.Input.PlayerInput.PointerDownForce();
    }

    public void PointerUpForce()
    {
        _hub.Input.PlayerInput.PointerUpForce();
    }

    public void PointerDownBrake()
    {
        _hub.Input.PlayerInput.PointerDownBrake();
    }

    public void PointerUpBrake()
    {
        _hub.Input.PlayerInput.PointerUpBrake();
    }

    public void PointerDownHandbrake()
    {
        _hub.Input.PlayerInput.PointerDownHandbrake();
    }

    public void PointerUpHandbrake()
    {
        _hub.Input.PlayerInput.PointerUpHandbrake();
    }

    public void SetButterflyCount(int count)
    {
        _butterflyIndicator.text = count.ToString();
        _butterfly.SetActive(count > 0);
    }

    private void Level_OnLevelComplete(int obj)
    {
        _jump.SetActive(false);
        _butterfly.SetActive(false);
        RaceButtonsHide();
    }

    private void Level_OnLevelLost()
    {
        _jump.SetActive(false);
        _butterfly.SetActive(false);
        RaceButtonsHide();
    }

    private void Level_OnLevelRestartFromCheckpoint()
    {
        _jump.SetActive(_hub.Game.Device == Device.Mobile);        
    }

    private void Game_OnScoreChanged(int score)
    {
        _scoreIndicator.text = (score - _scoreStartLevel).ToString();
    }

    private void RaceButtonsShow()
    {
        _buttonForce.SetActive(true);
        _buttonBrake.SetActive(true);
        _buttonHandbrake.SetActive(true);
        _buttonLeft.SetActive(true);
        _buttonRight.SetActive(true);
        _butterfly.SetActive(false);
    }

    private void RaceButtonsHide()
    {
        _buttonForce.SetActive(false);
        _buttonBrake.SetActive(false);
        _buttonHandbrake.SetActive(false);
        _buttonLeft.SetActive(false);
        _buttonRight.SetActive(false);
    }
}
