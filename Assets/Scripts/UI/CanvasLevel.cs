using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class CanvasLevel : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private WindowBase _windowSettings;
    //Do not use
    [SerializeField] private GameObject _buttonOpenPanel;
    [SerializeField] private GameObject _buttonPanelCloseScreen;
    [SerializeField] private GameObject _buttonCamera;
    [SerializeField] private GameObject _buttonMainMenu;
    [SerializeField] private GameObject _buttonOptions;
    [SerializeField] private GameObject _buttonRestart;    
    [SerializeField] private TMP_Text _scoreIndicator;    
    [SerializeField] private GameObject _buttonForce;
    [SerializeField] private GameObject _buttonBrake;
    [SerializeField] private GameObject _buttonHandbrake;
    [SerializeField] private GameObject _buttonLeft;
    [SerializeField] private GameObject _buttonRight;
    private int _scoreStartLevel;

    public Hub Hub => _hub;

    public PauseMenu PauseMenu => _pauseMenu;

    public WindowBase WindowSettings => _windowSettings;

    public void Init()
    {
        _pauseMenu.Init(_hub.Game);
        _windowSettings.Init(_hub.Game);
    }

    public void PausePanelShow()
    {
        Debug.Log("PausePanelShow()");
        _buttonPanelCloseScreen.SetActive(true);
        _buttonCamera.SetActive(true);
        _buttonMainMenu.SetActive(true);
        _buttonOptions.SetActive(true);
        _buttonRestart.SetActive(_hub.Level.IsPlaying);
        
        if (_hub.Level.IsRace) 
            RaceButtonsHide();        
    }

    public void PausePanelHide()
    {
        _buttonPanelCloseScreen.SetActive(false);
        _buttonCamera.SetActive(false);
        _buttonMainMenu.SetActive(false);
        _buttonOptions.SetActive(false);
        _buttonRestart.SetActive(false);        
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

    private void RaceButtonsShow()
    {
        _buttonForce.SetActive(true);
        _buttonBrake.SetActive(true);
        _buttonHandbrake.SetActive(true);
        _buttonLeft.SetActive(true);
        _buttonRight.SetActive(true);        
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
