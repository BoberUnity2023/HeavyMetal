using UnityEngine;
using UnityEngine.UI;

public class WindowSelectLevel : WindowBase
{
    [SerializeField] private MainMenuController _mainMenuController;
    [SerializeField] private Transform _canvas;
    [SerializeField] private GameObject _canvasScroll;
    [SerializeField] private RectTransform _content;
    [SerializeField] private ButtonLevel _buttonLevelPrefab;
    [SerializeField] private Transform[] _titles;
    [SerializeField] private Transform[] _locations;
    private const string _keyLocations = "Location";
    private int _selectedByGamepadLevel;

    private int LocationId
    {
        get { return PlayerPrefs.GetInt(_keyLocations); }
        set {  PlayerPrefs.SetInt(_keyLocations, value);}
    }

    protected override void Start()
    {
        base.Start();        
        
        CreateButtons();
        SetButtonLevels(_canvas);

        int id = LocationId;
        LevelLocation levelLocation = LevelLocationById(id);
        SetScreen(levelLocation);
    }

    public void PressLoadLevel(int level)
    {
        Game.Sound.Play(SoundClip.Click);

        if (IsLevelLock(level) && _mainMenuController.IsLevelAvialableByVideo(level))
        {
            return;
        }

        if (IsLevelLock(level))
            return;

        _mainMenuController.LoadLevel(level);
    }

    public void PressScreenNext()
    {
        int id = LocationId;
        LevelLocation levelLocation = LevelLocationById(id);

        if (levelLocation == LevelLocation.Paradize)
            SetScreen(LevelLocation.Alien);

        if (levelLocation == LevelLocation.SmokeCity)
            SetScreen(LevelLocation.Paradize);
    }

    public void PressScreenPrev()
    {
        int id = LocationId;
        LevelLocation levelLocation = LevelLocationById(id);

        if (levelLocation == LevelLocation.Paradize)
            SetScreen(LevelLocation.SmokeCity);

        if (levelLocation == LevelLocation.Alien)
            SetScreen(LevelLocation.Paradize);
    }

    public bool IsLevelLock(int level)
    {
        if (Game.Saves.GetPlayedLevels(level - 1))
            return false;

        int stars = Game.Saves.Stars + Game.Saves.PurchasedStars;
        return Game.ConfigLevels.Level(level).StarsForOpen > stars;
    }

    private void CreateButtons()
    {
        for (int i = 1; i <= Game.ConfigLevels.Levels.Length; i++)
        {
            Transform parent = LevelScreen(Game.ConfigLevels.Levels[i - 1].LevelLocation);
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
            case LevelLocation.Alien:
                return _locations[2];
        }
        return _locations[0];
    }

    private LevelLocation LevelLocationById(int id)
    {
        if (id == 0)
            return LevelLocation.SmokeCity;

        if (id == 1)
            return LevelLocation.Paradize;

        else
            return LevelLocation.Alien;
    }

    public void SetButtonLevels(Transform canvasScroll)
    {
        //int stars = YandexGame.savesData.Stars + YandexGame.savesData.PurchasedStars;
        //Debug.Log("Stars: " + stars);
        ButtonLevel[] buttonLevels = canvasScroll.GetComponentsInChildren<ButtonLevel>();

        for (int i = 0; i < buttonLevels.Length; i++)
        {
            Sprite icon = Game.ConfigLevels.Level(i + 1).Icon;
            buttonLevels[i].SetLevel(i + 1, icon, _mainMenuController);

            if (IsLevelLock(i + 1))
                buttonLevels[i].Lock();
        }
    }

    private void SetScreen(LevelLocation levelLocation)
    {        
        _locations[0].gameObject.SetActive(levelLocation == LevelLocation.SmokeCity);
        _locations[1].gameObject.SetActive(levelLocation == LevelLocation.Paradize);
        _locations[2].gameObject.SetActive(levelLocation == LevelLocation.Alien);

        _titles[0].gameObject.SetActive(levelLocation == LevelLocation.SmokeCity);
        _titles[1].gameObject.SetActive(levelLocation == LevelLocation.Paradize);
        _titles[2].gameObject.SetActive(levelLocation == LevelLocation.Alien);

        if (levelLocation == LevelLocation.SmokeCity)
            LocationId = 0;

        if (levelLocation == LevelLocation.Paradize)
            LocationId = 1;

        if (levelLocation == LevelLocation.Alien)
            LocationId = 2;

        _canvasScroll = _locations[LocationId].gameObject;
        _selectedByGamepadLevel = 0;
    }

    private void LoadScrollPosition()
    {
        float x = _content.anchoredPosition.x;
        float y = PlayerPrefs.GetInt("ScrollPosition");
        _content.anchoredPosition = new Vector2(x, y);
    }

    public void SelectLevelByGamepadNext()
    {
        Debug.Log("SelectLevelByGamepadNext()");
        ButtonLevel[] buttonLevels = _canvasScroll.GetComponentsInChildren<ButtonLevel>();
        Debug.Log("buttonLevels: " + buttonLevels.Length);
        _selectedByGamepadLevel++;
        if (_selectedByGamepadLevel > buttonLevels.Length)
            _selectedByGamepadLevel = 1;

        Button button = buttonLevels[_selectedByGamepadLevel - 1].GetComponent<Button>();
        button.Select();
    }

    public void SelectLevelByGamepadPrev()
    {
        Debug.Log("SelectLevelByGamepadPrev()");

        ButtonLevel[] buttonLevels = _canvasScroll.GetComponentsInChildren<ButtonLevel>();
        Debug.Log("buttonLevels: " + buttonLevels.Length);

        _selectedByGamepadLevel--;
        if (_selectedByGamepadLevel < 1)
            _selectedByGamepadLevel = buttonLevels.Length;

        Button button = buttonLevels[_selectedByGamepadLevel - 1].GetComponent<Button>();
        button.Select();
    }
}
