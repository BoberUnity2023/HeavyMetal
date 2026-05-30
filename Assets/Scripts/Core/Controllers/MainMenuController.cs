using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuController : SceneController
{    
    [SerializeField] private GameObject _canvasScroll;
    [SerializeField] private GameObject _windowLevels;
    [SerializeField] private GameObject _windowMain;
    [SerializeField] private GameObject _hero;
    [SerializeField] private GameObject _startHand;    
    [SerializeField] private Transform _canvas;
    [SerializeField] private Transform[] _titles;
    [SerializeField] private Transform[] _locations;
    [SerializeField] private ButtonLevel _buttonLevelPrefab;
    [SerializeField] private RectTransform _content;
    private const string _keyLocations = "Location";
    private int _selectedByGamepadLevel;

    private void OnEnable()
    {
        //int sceneIndex = Game != null ? Game.LastCompleteLevel : 2;
        //Game.SceneLoader.SceneIndex = sceneIndex;
        //SetButtonLevels();
        //if (Application.platform == RuntimePlatform.WindowsEditor)
        //{
        //    GameController game = FindObjectOfType<GameController>();
        //    if ( game == null)
        //    {
        //        SceneManager.LoadScene(0);
        //    }
        //}        
    }

    public void Init(GameController game, bool fromLevel = false)
    {
        //return;
        Game = game;
        CreateButtons();
        SetButtonLevels(_canvas);

        int id = PlayerPrefs.GetInt(_keyLocations);
        LevelLocation levelLocation = LevelLocationById(id);
        SetScreen(levelLocation);

        /*if (fromLevel) 
        {
            _windowMain.SetActive(false);
            _windowLevels.SetActive(true);
            _canvasScroll.SetActive(true);            
            LoadScrollPosition();            
        } */
    }

    public void LoadScene(int buildIndex)
    {        
        Game.SceneLoader.LoadScene(buildIndex);
    }

    public void OnLoadLevelByRewadedVideo(int level)
    {        
        LoadLevel(level);
    }

    public void PressLoadLevel(int level)
    {
        //Game.Sound.Play(SoundClip.Click);

        if (IsLevelLock(level) && IsLevelAvialableByVideo(level))
        {            
            return;
        }

        if (IsLevelLock(level))
            return;

        LoadLevel(level);
    } 
    
    private void LoadLevel(int level)
    {
        Debug.Log("Load Level: " + level);
        SaveScrollPosition();
        int buildIndex = Game.Levels.Level(level).SceneBuildIndex;//level + 1;
        Game.CurrentLevel = level;
        LoadScene(buildIndex);
    }

    public void PressLoadLastPlayedLevel() 
    {
        PressLoadLevel(Game.LastPlayedLevel);
    }

    public void SetButtonLevels(Transform canvasScroll)
    {
        //int stars = YandexGame.savesData.Stars + YandexGame.savesData.PurchasedStars;
        //Debug.Log("Stars: " + stars);
        ButtonLevel[] buttonLevels = canvasScroll.GetComponentsInChildren<ButtonLevel>();

        for (int i = 0; i < buttonLevels.Length; i++)
        {            
            Sprite icon = Game.Levels.Level(i + 1).Icon;
            buttonLevels[i].SetLevel(i + 1, icon, this);

            if (IsLevelLock(i + 1))
                buttonLevels[i].Lock();
        }
    }

    private void SetScreen(LevelLocation levelLocation)
    {
        _locations[0].gameObject.SetActive(levelLocation == LevelLocation.SmokeCity);
        _locations[1].gameObject.SetActive(levelLocation == LevelLocation.Paradize);

        _titles[0].gameObject.SetActive(levelLocation == LevelLocation.SmokeCity);
        _titles[1].gameObject.SetActive(levelLocation == LevelLocation.Paradize);

        if (levelLocation == LevelLocation.SmokeCity)
            PlayerPrefs.SetInt(_keyLocations, 0);

        if (levelLocation == LevelLocation.Paradize)
            PlayerPrefs.SetInt(_keyLocations, 1);

        _selectedByGamepadLevel = 0;
    }

    public void PressScreenNext()
    {
        SetScreen(LevelLocation.Paradize);
    }

    public void PressScreenPrev()
    {
        SetScreen(LevelLocation.SmokeCity);
    }

    public void SelectLevelByGamepadNext()
    {
        ButtonLevel[] buttonLevels = _canvasScroll.GetComponentsInChildren<ButtonLevel>();

        _selectedByGamepadLevel++;
        if (_selectedByGamepadLevel > buttonLevels.Length)
            _selectedByGamepadLevel = 1;
        
        Button button = buttonLevels[_selectedByGamepadLevel - 1].GetComponent<Button>();
        button.Select();
    }

    public void SelectLevelByGamepadPrev()
    {
        ButtonLevel[] buttonLevels = _canvasScroll.GetComponentsInChildren<ButtonLevel>();

        _selectedByGamepadLevel--;
        if (_selectedByGamepadLevel < 1)
            _selectedByGamepadLevel = buttonLevels.Length;

        Button button = buttonLevels[_selectedByGamepadLevel - 1].GetComponent<Button>();
        button.Select();
    }

    public void PressSound()
    {
        Game.Sound.Play(SoundClip.Click);
    }

    private void CreateButtons()
    {
        for (int i = 1; i <= Game.Levels.Levels.Length; i++)        
        {
            Transform parent = LevelScreen(Game.Levels.Levels[i - 1].LevelLocation);
            ButtonLevel buttonLevel = Instantiate(_buttonLevelPrefab, parent);
            int iCopy = i;
            buttonLevel.GetComponent<Button>().onClick.AddListener(() => PressLoadLevel(iCopy));
        }
    }

    private Transform LevelScreen(LevelLocation levelLocation)
    {
        switch (levelLocation)
        {
            case LevelLocation.SmokeCity:
                return _locations[0];
            case LevelLocation.Paradize: 
                return _locations[1];
        }
        return _locations[0];
    }

    public bool IsLevelLock(int level)
    {
        if (Game.Saves.GetPlayedLevels(level - 1))
            return false;

        int stars = Game.Saves.Stars + Game.Saves.PurchasedStars;
        return Game.Levels.Level(level).StarsForOpen > stars;
    }

    public bool IsLevelAvialableByVideo(int level)
    {

        if (!IsLevelLock(level))
            return false;

        if (!Game.IsTutorialShown)
            return false;

        //int stars = Game.Saves.Stars + Game.Saves.PurchasedStars;
        //if (Game.Levels.Level(level).StarsForOpen <= stars + 9)
        //    return true;

        return false;
    }

    private void SaveScrollPosition()
    {
        if (_content != null)//Del
            PlayerPrefs.SetInt("ScrollPosition", (int)_content.anchoredPosition.y);
    }

    private void LoadScrollPosition()
    {
        float x = _content.anchoredPosition.x;
        float y = PlayerPrefs.GetInt("ScrollPosition");
        _content.anchoredPosition = new Vector2(x, y);        
    }

    private LevelLocation LevelLocationById(int id)
    {
        if (id == 0)
            return LevelLocation.SmokeCity;

        return LevelLocation.Paradize;
    }
}
