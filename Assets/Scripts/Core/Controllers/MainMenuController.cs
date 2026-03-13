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
    [SerializeField] private Transform _canvasScrollContent;
    [SerializeField] private ButtonLevel _buttonLevelPrefab;
    [SerializeField] private RectTransform _content;
    [SerializeField] private Hat _hat;    

    public Hat Hat => _hat;    

    private void OnEnable()
    {
        //int sceneIndex = Game != null ? Game.LastCompleteLevel : 2;
        //Game.SceneLoader.SceneIndex = sceneIndex;
        //SetButtonLevels();
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            GameController game = FindObjectOfType<GameController>();
            if ( game == null)
            {
                SceneManager.LoadScene(0);
            }
        }        
    }

    public void Init(GameController game, bool fromLevel = false)
    {
        Game = game;
        CreateButtons();
        SetButtonLevels();
        if (fromLevel) 
        {
            _windowMain.SetActive(false);
            _windowLevels.SetActive(true);
            _canvasScroll.SetActive(true);            
            LoadScrollPosition();
            
        }  
    }

    public void LoadScene(int buildIndex)
    {        
        Game.SceneLoader.LoadScene(buildIndex);
    }

    public void OnLoadLevelByRewadedVideo(int level)
    {        
        LoadLevel(level);
    }

    private void PressLoadLevel(int level)
    {
        Game.Sound.Play(SoundClip.Click);

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
        SaveScrollPosition();
        int buildIndex = Game.Levels.Level(level).SceneBuildIndex;//level + 1;
        Game.CurrentLevel = level;
        LoadScene(buildIndex);
    }

    public void PressLoadLastPlayedLevel() 
    {
        PressLoadLevel(Game.LastPlayedLevel);
    }

    public void SetButtonLevels()
    {
        //int stars = YandexGame.savesData.Stars + YandexGame.savesData.PurchasedStars;
        //Debug.Log("Stars: " + stars);
        ButtonLevel[] buttonLevels = _canvasScroll.GetComponentsInChildren<ButtonLevel>();

        for (int i = 0; i < buttonLevels.Length; i++)
        {            
            Sprite icon = Game.Levels.Level(i + 1).Icon;
            buttonLevels[i].SetLevel(i + 1, icon, this);

            if (IsLevelLock(i + 1))
                buttonLevels[i].Lock();
        }
    }

    public void PressSound()
    {
        Game.Sound.Play(SoundClip.Click);
    }

    private void CreateButtons()
    {
        for (int i = 1; i <= Game.Levels.Levels.Length; i++)        
        {
            ButtonLevel buttonLevel = Instantiate(_buttonLevelPrefab, _canvasScrollContent);
            int iCopy = i;
            buttonLevel.GetComponent<Button>().onClick.AddListener(() => PressLoadLevel(iCopy));
        }
    }

    public bool IsLevelLock(int level)
    {
        //if (Game.Saves.GetPlayedLevels(level - 1))
        //    return false;

        //int stars = Game.Saves.Stars + Game.Saves.PurchasedStars;
        return true;// Game.Levels.Level(level).StarsForOpen > stars;
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
        PlayerPrefs.SetInt("ScrollPosition", (int)_content.anchoredPosition.y);
    }

    private void LoadScrollPosition()
    {
        float x = _content.anchoredPosition.x;
        float y = PlayerPrefs.GetInt("ScrollPosition");
        _content.anchoredPosition = new Vector2(x, y);        
    }
}
