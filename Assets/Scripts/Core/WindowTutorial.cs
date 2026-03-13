using UnityEngine;
using UnityEngine.UI;

public class WindowTutorial : MonoBehaviour
{
    [SerializeField] private Hub _hub;
    [SerializeField] private GameObject _windowMove;    
    [SerializeField] private GameObject _windowJump;
    [SerializeField] private Button _buttonNextMove;
    [SerializeField] private Button _buttonNextJump;
    [SerializeField] private GameObject _pressIndicator;
    [SerializeField] private GameObject[] _mobileObjects;
    [SerializeField] private GameObject[] _desktopObjects; 

    private bool IsTutorialShown
    {
        get { return _hub.Game.IsTutorialShown; }        
    }
    
    private void Start()
    {
        if (!IsTutorialShown)
        {            
            _windowMove.SetActive(true);
            _hub.Joistick.gameObject.SetActive(false);
            _buttonNextMove.onClick.AddListener(PressNextMove);
            _buttonNextJump.onClick.AddListener(PressNextJump);
            SetObjectsByDevice();
            _hub.Analitycs.SendTutorialStart();
        }

        if (_hub.Game.CurrentLevel == 19 &&
            _hub.Game.Device == Device.Desktop &&
            IsTutorialShown &&
            PlayerPrefs.GetInt("Jumped", 0) == 0
            )
        {
            _buttonNextJump.onClick.AddListener(PressNextJump);
            _windowJump.SetActive(true);
        }
    }

    private void PressNextMove()
    {
        _windowMove.SetActive(false);

        if (_hub.Game.Device == Device.Desktop)
            _windowJump.SetActive(true);

        if (_hub.Game.Device == Device.Mobile)
            TutorialComplete();
    }

    private void PressNextJump()
    {
        _windowJump.SetActive(false);
        if (!IsTutorialShown)
            TutorialComplete();
    }

    private void SetObjectsByDevice()
    {
        foreach (var obj in _mobileObjects)
        {
            obj.SetActive(_hub.Game.Device == Device.Mobile);
        }

        foreach (var obj in _desktopObjects)
        {
            obj.SetActive(_hub.Game.Device == Device.Desktop);
        }

        if (_hub.Game.Device == Device.Desktop)
            _pressIndicator.transform.localScale = Vector3.zero;
    }

    private void TutorialComplete()
    {
        _hub.Joistick.gameObject.SetActive(true);        
        _hub.Analitycs.SendTutorialComplete();
    }
}
