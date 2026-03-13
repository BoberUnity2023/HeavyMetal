using Coffee.UIEffects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ButtonLevel : MonoBehaviour
{
    [SerializeField] private UIStar[] _uIStars;
    [SerializeField] private TMP_Text _levelIndicator;    
    [SerializeField] private Image _icon;
    [SerializeField] private Image _iconLight;
    [SerializeField] private UIEffect _iconUIEffect;
    [SerializeField] private GameObject _lock;
    [SerializeField] private GameObject _plus;
    [SerializeField] private GameObject _video;
    private GameController _game;
    private MainMenuController _mainMenuController;
    private int _level;

    public void SetLevel(int level, Sprite icon, MainMenuController mainMenuController)
    {
        _mainMenuController = mainMenuController;
        _game = mainMenuController.Game;
        _level = level;
        //Debug.Log(gameObject.name + ".SetLevel(" + level + ")");
        _levelIndicator.text = level.ToString();

        //int stars = _game.Saves.GetLevelStars(level - 1);
        //SetStars(stars);

        _icon.sprite = icon;
        _iconLight.sprite = icon;
        _lock.SetActive(false);
        _video.SetActive(mainMenuController.IsLevelAvialableByVideo(level));

        bool isLevelplayed = IsLevelPlayed(level);
        _plus.SetActive(!isLevelplayed);        
        _iconLight.gameObject.SetActive(!isLevelplayed);        
    }

    private void SetStars(int count)
    {
        //Debug.Log(gameObject.name + ".SetStars(" + stars + ")");
        for (int i = 0; i < _uIStars.Length; i++)
        {
            if (count > i)
                _uIStars[i].On();
            else
                _uIStars[i].Off();
        }
    }

    public void Lock()
    {
        _iconUIEffect.enabled = true;
        _iconUIEffect.effectFactor = 0.6f; 
        _lock.SetActive(true);
        _plus.SetActive(false);
        _video.SetActive(_mainMenuController.IsLevelAvialableByVideo(_level));
        _iconLight.gameObject.SetActive(false);
        for (int i = 0; i < _uIStars.Length; i++)
        {
            _uIStars[i].gameObject.SetActive(false);
        }
    }

    private bool IsLevelPlayed(int level)
    {
        return true;// _game.Saves.GetPlayedLevels(level - 1);
    }
}
